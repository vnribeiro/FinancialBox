using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinancialBox.Application.Contracts.Services;

namespace FinancialBox.Presentation.Services;

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    // Your JWT uses "sub" as the user id
    public string? UserId =>
        Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    // Your JWT uses "email"
    public string? Email =>
        Principal?.FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? Principal?.FindFirstValue(ClaimTypes.Email);

    // You currently don't add a name claim, so this will likely be null
    public string? Name =>
        Principal?.FindFirstValue(JwtRegisteredClaimNames.Name)
        ?? Principal?.FindFirstValue(ClaimTypes.Name);

    // Your JWT uses standard "role" claims
    public IReadOnlyList<string> Roles =>
        Principal?.FindAll(ClaimTypes.Role)
            .Select(r => r.Value)
            .Distinct()
            .ToList()
        ?? [];

    // You generate JTI
    public string? Jti =>
        Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);

    // These come from JWT standard numeric date claims (Unix time seconds)
    public DateTimeOffset? IssuedAt => GetUnixTimeClaim(JwtRegisteredClaimNames.Iat);
    public DateTimeOffset? ExpiresAt => GetUnixTimeClaim(JwtRegisteredClaimNames.Exp);

    public TimeSpan? TimeToExpire
    {
        get
        {
            if (ExpiresAt is null) return null;

            var remaining = ExpiresAt.Value - DateTimeOffset.UtcNow;
            return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
        }
    }

    private DateTimeOffset? GetUnixTimeClaim(string claimType)
    {
        var raw = Principal?.FindFirstValue(claimType);
        if (string.IsNullOrWhiteSpace(raw)) return null;

        return long.TryParse(raw, out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds)
            : null;
    }
}
