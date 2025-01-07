using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AcceralytDevTest.models;
using Microsoft.IdentityModel.Tokens;

namespace AcceralytDevTest.utils;

public class TokenService:ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly JwtSettings _jwtSettings;

    public TokenService(ILogger<TokenService> logger, JwtSettings jwtSettings)
    {
        _logger = logger;
        _jwtSettings = jwtSettings;
    }

    public string GenerateToken(User user)
    {
        try
        {
            _logger.LogInformation($"Generating token for user {user.Email}");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            _logger.LogInformation("Token generated successfully.");
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new SecurityTokenException("Failed to generate token.");
        }
    }
}