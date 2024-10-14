using ConvertCash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

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
        public IActionResult ViewMain (string coinBase) 
        {
            var requestWeb = WebRequest.CreateHttp(baseAddress + coinBase);
            requestWeb.Method = "GET";
            requestWeb.UserAgent = "RequestWebDemo";

            Models.ConvertCash take = new Models.ConvertCash("", "", [ ]);

            using (var result = requestWeb.GetResponse())
            {
                var streamData = result.GetResponseStream();
                StreamReader reader = new StreamReader(streamData);
                object data = reader.ReadToEnd();

                take = JsonConvert.DeserializeObject<Models.ConvertCash>(data.ToString());
            }

            ViewBag.coinBase = take;

            return View("HomeSite");   
        }

    }
}
