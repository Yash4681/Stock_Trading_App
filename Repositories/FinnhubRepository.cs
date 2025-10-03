using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _httpClientFactory = clientFactory;
            _client = _httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            Stream stream = responseMessage.Content.ReadAsStream();
            StreamReader streamReader = new StreamReader(stream);
            string data = await streamReader.ReadToEndAsync();
            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(data);

            return keyValuePairs;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            Stream stream = responseMessage.Content.ReadAsStream();
            StreamReader streamReader = new StreamReader(stream);
            string data = await streamReader.ReadToEndAsync();
            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(data);

            return keyValuePairs;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            Stream stream = responseMessage.Content.ReadAsStream();
            StreamReader streamReader = new StreamReader(stream);
            string data = await streamReader.ReadToEndAsync();
            List<Dictionary<string, string>>? keyValuePairs = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data);

            return keyValuePairs;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            Stream stream = responseMessage.Content.ReadAsStream();
            StreamReader streamReader = new StreamReader(stream);
            string data = await streamReader.ReadToEndAsync();
            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(data);

            return keyValuePairs;
        }
    }
}
