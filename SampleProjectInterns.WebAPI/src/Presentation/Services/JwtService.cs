using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Application.Dtos.Identities.Response;
using SampleProjectInterns.WebAPI.Presentation.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleProjectInterns.WebAPI.Presentation.Services;

public class JwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GenerateToken(IdentityDto identity)
    {
        var key = Encoding.ASCII.GetBytes(_settings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                //claimlar ile veriler taşınır
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, identity.Email),
                new Claim(ClaimTypes.Name, identity.Email),
                new Claim(ClaimTypes.Role,  identity.Role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             }),
            //token süresini ayarlama
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        //token üretir
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}