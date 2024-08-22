using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock> AddAsync(Stock stock)
        {
            await _dbContext.Set<Stock>().AddAsync(stock);
            await _dbContext.SaveChangesAsync();    
            return stock;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
            var stock = await _dbContext.Set<Stock>().FindAsync(id);

            _dbContext.Set<Stock>().Remove(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _dbContext.Set<Stock>()
                                   .Include(s => s.Comments)
                                   .ThenInclude(c => c.AppUser)
                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if(!string.IsNullOrEmpty(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.IsDescendant)
                {
                    stocks = stocks.OrderByDescending(GetSortProperty(query));
                }
                else
                {
                    stocks = stocks.OrderBy(GetSortProperty(query));
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber)
                               .Take(query.PageSize)
                               .ToListAsync();
        }

        public async Task<Stock> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Stock>()
                                   .Include(s => s.Comments)
                                   .ThenInclude(c => c.AppUser)
                                   .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Set<Stock>().FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _dbContext.Set<Stock>().AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var entry = await _dbContext.Set<Stock>().FindAsync(id);

            if(entry is null)
            {
                return null;
            }

            stock.Id = id;

            _dbContext.Entry<Stock>(entry).CurrentValues.SetValues(stock);

            await _dbContext.SaveChangesAsync();

            return stock;
        }

        #region Private Methods
        private Expression<Func<Stock, object>> GetSortProperty(QueryObject query)
        {
            Expression<Func<Stock, object>> keySelector = query.SortBy?.ToLower() switch
            {
                "symbol" => stock => stock.Symbol,
                "companyname" => stock => stock.CompanyName,
                "purchase" => stock => stock.CompanyName,
                "lastdiv" => stock => stock.CompanyName,
                "industry" => stock => stock.Industry,
                "marketcap" => stock => stock.Industry,
                _ => stock => stock.Id
            };

            return keySelector;
        }
        #endregion
    }
}
