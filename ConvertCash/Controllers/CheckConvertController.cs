using ConvertCash.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;

namespace ConvertCash.Controllers
{
    public class CheckConvertController : Controller
    {
        Uri baseAddress = new Uri("https://v6.exchangerate-api.com/v6/d5f02a67d1cffe56f08a96b8/latest/");
        private readonly HttpClient _client;
        private IWebHostEnvironment _hostingEnvironment;

        public CheckConvertController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View("HomeSite");
        }


        [HttpPost]
        public IActionResult Index(string tempBaseCurrency, string tempTargetCurrency, double tempValueForConverte)
        {

            ViewBag.tempValueForConverte = tempValueForConverte.ToString();

            string baseCurrency = tempBaseCurrency.ToUpper();
            string targetCurrency = tempTargetCurrency.ToUpper();

            var requestWeb = WebRequest.CreateHttp(baseAddress + baseCurrency);
            requestWeb.Method = "GET";
            requestWeb.UserAgent = "RequestWebDemo";

            API_Obj allcontent;

            using (var result = requestWeb.GetResponse())
            {
                var streamData = result.GetResponseStream();

                using (var reader = new StreamReader(streamData))
                {
                    var data = reader.ReadToEnd();

                    allcontent = JsonConvert.DeserializeObject<API_Obj>(data);
                }
            }

            if (allcontent.conversion_rates != null && allcontent.conversion_rates.GetType().GetProperty(targetCurrency) != null)
            {
                var targetRate = (double)allcontent.conversion_rates.GetType().GetProperty(targetCurrency).GetValue(allcontent.conversion_rates);

                ViewBag.ConversionRate = targetRate;
                ViewBag.TargetCurrency = targetCurrency;

                tempValueForConverte = tempValueForConverte * targetRate;

                string valueForConverte = tempValueForConverte.ToString();

                if (valueForConverte.Length >= 7)
                {
                    valueForConverte = valueForConverte.Substring(0, 7);   
                }

                ViewBag.valueForConverte = valueForConverte;

                ViewBag.showValue = "Valor Convertido: " + valueForConverte + " " + targetCurrency;

            }
            else
            {
                ViewBag.ConversionRate = null;
                ViewBag.Error = "Moeda de destino não encontrada.";
            }

            ViewBag.BaseCurrency = baseCurrency;
            ViewBag.ConversionRates = allcontent;

            return View("HomeSite");
        }

    }
}


