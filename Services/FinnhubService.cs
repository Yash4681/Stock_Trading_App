using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubService> _logger;

        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
        {            
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile method is called from FinnhubService");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            return await _finnhubRepository.GetCompanyProfile(stockSymbol);
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            _logger.LogInformation("GetStocks method is called from FinnhubService");

            return await _finnhubRepository.GetStocks();
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            _logger.LogInformation("GetStockPriceQuote method is called from FinnhubService");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            return await _finnhubRepository.GetStockPriceQuote(stockSymbol);
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            _logger.LogInformation("SearchStocks method is called from FinnhubService");
            _logger.LogDebug($"stockSymbolToSearch: {stockSymbolToSearch}");

            return await _finnhubRepository.SearchStocks(stockSymbolToSearch);
        }
    }
}
