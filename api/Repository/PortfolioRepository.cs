using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PortfolioRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Portfolio> AddAsync(Portfolio portfolio)
        {
            await _dbContext.Set<Portfolio>().AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(AppUser user, string symbol)
        {
            var stockFromPortfolioToBeDeleted = await _dbContext.Set<Portfolio>()
                                                          .FirstOrDefaultAsync(p => p.AppUserId == user.Id &&
                                                                                    p.Stock.Symbol.ToLower() == symbol.ToLower());

            if (stockFromPortfolioToBeDeleted is null)
            {
                return null;
            }

            _dbContext.Set<Portfolio>().Remove(stockFromPortfolioToBeDeleted);
            await _dbContext.SaveChangesAsync();

            return stockFromPortfolioToBeDeleted;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Set<Portfolio>().Where(p => p.AppUserId == user.Id)
                                                    .Select(stock => new Stock()
                                                    {
                                                        Id = stock.StockId,
                                                        Symbol = stock.Stock.Symbol,
                                                        CompanyName = stock.Stock.CompanyName,
                                                        Purchase = stock.Stock.Purchase,
                                                        LastDiv = stock.Stock.LastDiv,  
                                                        Industry = stock.Stock.Industry,
                                                        MarketCap = stock.Stock.MarketCap
                                                    })
                                                    .ToListAsync();
        }
    }
}
