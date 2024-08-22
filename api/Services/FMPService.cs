using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FMPService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}");

                if(result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var task = JsonConvert.DeserializeObject<FMPStock[]>(content);

                    var stock = task[0];

                    if(stock is null)
                    {
                        return null;
                    }

                    return stock.ToStockFromFMPStock();
                }

                return null;
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex}");
                return null;
            }
        }
    }
}
