namespace ConvertCash.Models
{
    public record ConvertCash(string result, string base_code, Dictionary<string, decimal> Conversion_rates){}
}
