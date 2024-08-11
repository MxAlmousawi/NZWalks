using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Interfaces;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager , ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto )
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                }
                if (identityResult.Succeeded)
                {
                    return Ok("User was registered! pleases login");
                }
            }
            return BadRequest("Something went wrong!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user == null)
            {
                return BadRequest("Incorrct username or password");
            }
            var checkPassword = await userManager.CheckPasswordAsync(user , loginRequestDto.Password);
            if(checkPassword == false)
            {
                return BadRequest("Incorrct username or password");
            }
            var roles = await userManager.GetRolesAsync(user);

            if(roles == null)
            {
                return BadRequest("Incorrct username or password");
            }

            var jwtToken = tokenRepository.CreateJWTToken(user , roles.ToList());

            var response = new LoginResponseDto()
            {
                JwtToken = jwtToken
            };
            return Ok(response);
        }
    }
}
