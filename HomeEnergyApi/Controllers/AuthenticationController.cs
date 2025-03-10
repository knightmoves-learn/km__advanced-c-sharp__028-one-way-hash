using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeEnergyApi.Dtos;
using HomeEnergyApi.Models;


namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;
        private readonly IUserRepository userRepository;
        private readonly ValueHasher passwordHasher;
        private readonly IMapper mapper;

        public AuthenticationController(IConfiguration configuration,
                                        IUserRepository userRepository,
                                        ValueHasher passwordHasher,
                                        IMapper mapper)
        {
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _secret = configuration["Jwt:Secret"];
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var existingUser = userRepository.FindByUsername(userDto.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken.");
            }

            var user = mapper.Map<User>(userDto);
            user.HashedPassword = passwordHasher.HashPassword(userDto.Password);

            userRepository.Save(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] UserDto userDto)
        {
            var user = userRepository.FindByUsername(userDto.Username);
            if (user == null || !passwordHasher.VerifyPassword(user.HashedPassword, userDto.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            string token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
