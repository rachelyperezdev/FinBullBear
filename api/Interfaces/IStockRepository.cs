using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> AddAsync(Stock stock);
        Task<Stock> DeleteAsync(int id);
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<bool> StockExists(int id);
        Task<Stock> UpdateAsync(int id, Stock stock);
        
    }
}
