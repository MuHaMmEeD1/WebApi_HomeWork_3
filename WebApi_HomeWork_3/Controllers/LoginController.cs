using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_HomeWork_3.Data;
using WebApi_HomeWork_3.Data.Abstract;
using WebApi_HomeWork_3.Data.Concrete;
using WebApi_HomeWork_3.Dtos;
using WebApi_HomeWork_3.Entitys;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_HomeWork_3.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
      

        private readonly IAutoRepository _autoRepository;
        private readonly IConfiguration _configuration;

        public LoginController(IAutoRepository autoRepository, IConfiguration configuration)
        {
            _autoRepository = autoRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("test1")]   
        public string Test1()
        {
            return "Test1 OK";
        }







        [HttpGet("test2")]   
        public string Test2()
        {
            return "OK";
        }






        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _autoRepository.Login(dto.UserName, dto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Key").Value);

            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Username)

                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)


            };

            var token = tokenHandler.CreateToken(tokenDes);

            var tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] RegisterDto dto)
        {

            if (await _autoRepository.UserExists(dto.UserName))
            {
                ModelState.AddModelError("UserName", "Error UserName");

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCreate = new User
            {
                Username = dto.UserName,
            };

            await _autoRepository.Register(userCreate, dto.Password);
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
