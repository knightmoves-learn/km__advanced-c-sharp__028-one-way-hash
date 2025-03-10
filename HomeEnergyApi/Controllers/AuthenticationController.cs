using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeEnergyApi.Dtos;


namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;

        public AuthenticationController(IConfiguration configuration)
        {
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _secret = configuration["Jwt:Secret"];
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] TokenDto tokenDto)
        {
            if (string.IsNullOrEmpty(tokenDto.Role))
            {
                return BadRequest("Role is required.");
            }

            string token = GenerateJwtToken(tokenDto.Role);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, "maria@knightmove.org"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
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
