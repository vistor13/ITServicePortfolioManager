using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto.Auth;
using ITServicePortfolioManager.DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ITServicePortfolioManager.BLL.Services;

public class JwtService(IOptions<AuthOptions> option) : IJwtService
{
    private readonly AuthOptions _options = option.Value;

    public string GenerateToken(UserEntity user)
    {
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
