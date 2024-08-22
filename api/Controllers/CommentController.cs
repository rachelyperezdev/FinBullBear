using api.DTOs.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepository, 
                                 IStockRepository stockRepository,
                                 UserManager<AppUser> userManager,
                                 IFMPService fmpService)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {
            var comments = await _commentRepository.GetAllAsync(queryObject);

            if(comments.Any())
            {
                var commentsDTO = comments.Select(c => c.ToCommentDTO());

                return Ok(commentsDTO);
            }

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if(comment == null)
                return NoContent();

            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentRequestDTO commentDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var comment = commentDTO.ToCommentFromCreate(stock.Id);
            comment.AppUserId = user.Id;

            var savedComment = await _commentRepository.AddAsync(comment);

            if (savedComment is null)
            {
                return BadRequest("An error occurred while saving the comment.");
            }

            return CreatedAtAction(nameof(GetById), new { id = savedComment.Id }, comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);

            if(comment is null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO commentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedComment = await _commentRepository.UpdateAsync(id, commentDTO.ToCommentFromUpdate());

            if (updatedComment is null)
            {
                return BadRequest(ModelState);
            }

            return Ok(commentDTO);
        }
        
    }
}
