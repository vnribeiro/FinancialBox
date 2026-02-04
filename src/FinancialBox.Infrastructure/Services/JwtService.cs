using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Domain.Features.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinancialBox.Infrastructure.Services;

internal sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(User user, IEnumerable<Claim>? extraClaims = null)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Address),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (extraClaims is not null)
            claims.AddRange(extraClaims);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpirationTime),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
