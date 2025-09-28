using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {            
            _HttpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {            
            HttpClient httpClient = _HttpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={_configuration["TradingOptions:DefaultStockSymbol"]}&token={_configuration["FinnhubToken"]}"));
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(stream);
            string response = await reader.ReadToEndAsync();

            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
            return keyValuePairs;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
        {
            HttpClient httpClient = _HttpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://finnhub.io/api/v1/quote?symbol={_configuration["TradingOptions:DefaultStockSymbol"]}&token={_configuration["FinnhubToken"]}"));
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(stream);
            string response = await reader.ReadToEndAsync();

            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
            return keyValuePairs;
        }
    }
}
