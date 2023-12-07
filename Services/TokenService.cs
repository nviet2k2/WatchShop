using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace Services;
public interface ITokenService
{
    string GenerateToken(string email);
    bool ValidateToken(string token, UserModel user);
}

public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;

    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["JWT:Secret"];
        _issuer = configuration["JWT:ValidIssuer"];
    }

    public string GenerateToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Convert.FromBase64String(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _issuer,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token, UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_secretKey));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
