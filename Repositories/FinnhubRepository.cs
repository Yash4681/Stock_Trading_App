using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FinnhubRepository> _logger;

        public FinnhubRepository(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<FinnhubRepository> logger)
        {
            _httpClientFactory = clientFactory;
            _client = _httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile method is called from FinnhubRepository");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

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
            _logger.LogInformation("GetStockPriceQuote method is called from FinnhubRepository");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

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
            _logger.LogInformation("GetStocks method is called from FinnhubRepository");

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
            _logger.LogInformation("SearchStocks method is called from FinnhubRepository");
            _logger.LogDebug($"stockSymbolToSearch: {stockSymbolToSearch}");
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
