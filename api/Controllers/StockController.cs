using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepository.GetAllAsync(query);

            if (stocks.Any())
            {
                var stocksDTO = stocks.Select(stock => stock.ToStockDTO()).ToList();

                return Ok(stocksDTO);
            }
            
            return NoContent();
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if(stock == null)
                return NoContent();

            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = stockDTO.ToStockFromCreate();

            var savedStock = await _stockRepository.AddAsync(stock);

            if(savedStock is not null)
            {
                return CreatedAtAction(nameof(GetById), new {id = stock.Id}, stock.ToStockDTO());
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO stockDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedStock = await _stockRepository.UpdateAsync(id, stockDTO.ToStockFromUpdateDTO());

            if(updatedStock is null)
            {
                return BadRequest("Error updating the stock");
            }

            return Ok(updatedStock.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize] 
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if(stock is null)
            {
                return NotFound();
            }

            await _stockRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
