using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<Portfolio> AddAsync(Portfolio portfolio);
        Task<Portfolio> DeleteAsync(AppUser user, string symbol);
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        
    }
}
