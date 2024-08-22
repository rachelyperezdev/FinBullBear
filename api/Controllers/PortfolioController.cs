using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager, 
                                   IPortfolioRepository portfolioRepository,
                                   IStockRepository stockRepository,
                                   IFMPService fmpService)
        {
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var portfolio = await _portfolioRepository.GetUserPortfolio(user);

            if(portfolio.Any())
            {
                return Ok(portfolio);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if(stock is null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);

                if(stock is null)
                {
                    return BadRequest("Stock doesn't exists");
                }
                else
                {
                    await _stockRepository.AddAsync(stock);
                }
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);

            if(userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower())) 
            {
                return BadRequest("Stock is already in portfolio.");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id
            };

            var createdPortfolio =await _portfolioRepository.AddAsync(portfolioModel);

            if(createdPortfolio is null)
            {
                return BadRequest("Error creating portfolio");
            }

            return CreatedAtAction(nameof(GetUserPortfolio), new { id = new { appUserId = user.Id, stockId = stock.Id } });
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var portfolio = await _portfolioRepository.GetUserPortfolio(user);
            var stockToBeDeleted = portfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(stockToBeDeleted.Any())
            {
                var deletedStockFromPortfolio = await _portfolioRepository.DeleteAsync(user, symbol);

                if(deletedStockFromPortfolio is null)
                {
                    return BadRequest("An error occurred while deleting the stock.");
                }
                
                return NoContent();
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }
        }
    }
}
