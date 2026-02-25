using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Domain.Features.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtOptions = FinancialBox.Infrastructure.Options.JwtOptions;

namespace FinancialBox.Infrastructure.Services;

internal sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;

    public TokenResponse GenerateToken(User user)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Address),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var roles = user.Roles.Select(r => r.Name);
        claims.AddRange(roles.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().
            Select(role => new Claim(ClaimTypes.Role, role)));

        var expiresAtUtc = DateTime.UtcNow.AddHours(_options.ExpiresInHours);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new TokenResponse(accessToken, expiresAtUtc);
    }
}
