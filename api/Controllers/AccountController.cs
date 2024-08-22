using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                AppUser user = new AppUser()
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email
                };

                var creatededUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if(creatededUser.Succeeded)
                {
                    var rolesResult = await _userManager.AddToRoleAsync(user, api.Enums.Roles.User.ToString());

                    if(rolesResult.Succeeded)
                    {
                        return Ok(new NewUserDTO
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        });
                    }
                    else
                    {
                        return BadRequest(rolesResult.Errors);
                    }
                    
                }
                else
                {
                    return BadRequest(creatededUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(loginDTO.UserName);

            if(user is null)
            {
                return Unauthorized("User not valid");
            }

            var result =  await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (result.Succeeded)
            {
                return Ok(new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
            }
            else
            {
                return Unauthorized("User not found or incorrect password!");
            }
        }
    }
}
