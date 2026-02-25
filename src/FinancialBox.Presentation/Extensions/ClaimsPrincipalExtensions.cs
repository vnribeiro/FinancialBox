using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinancialBox.Application.Features.Auth.Errors;

namespace FinancialBox.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal? user) =>
        user?.Identity?.IsAuthenticated ?? false;

    public static bool TryGetUserIdAsGuid(this ClaimsPrincipal? user, out Guid userId)
    {
        var raw = user.GetUserId();
        return Guid.TryParse(raw, out userId);
    }

    public static string? GetUserId(this ClaimsPrincipal? user) =>
        user?.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? user?.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetEmail(this ClaimsPrincipal? user) =>
        user?.FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? user?.FindFirstValue(ClaimTypes.Email);

    public static string? GetName(this ClaimsPrincipal? user) =>
        user?.FindFirstValue(JwtRegisteredClaimNames.Name)
        ?? user?.FindFirstValue(ClaimTypes.Name);

    public static IReadOnlyList<string> GetRoles(this ClaimsPrincipal? user) =>
        user?.FindAll(ClaimTypes.Role)
            .Select(role => role.Value )
            .Distinct()
            .ToList()
        ?? [];

    public static string? GetJti(this ClaimsPrincipal? user) =>
        user?.FindFirstValue(JwtRegisteredClaimNames.Jti);

    public static DateTimeOffset? GetIssuedAt(this ClaimsPrincipal? user) =>
        user.GetUnixTimeClaim(JwtRegisteredClaimNames.Iat);

    public static DateTimeOffset? GetExpiresAt(this ClaimsPrincipal? user) =>
        user.GetUnixTimeClaim(JwtRegisteredClaimNames.Exp);

    public static TimeSpan? GetTimeToExpire(this ClaimsPrincipal? user)
    {
        var expiresAt = user.GetExpiresAt();
        if (expiresAt is null)
            return null;

        var remaining = expiresAt.Value - DateTimeOffset.UtcNow;
        return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
    }

    private static DateTimeOffset? GetUnixTimeClaim(this ClaimsPrincipal? user, string claimType)
    {
        var rawValue = user?.FindFirstValue(claimType);
        if (string.IsNullOrWhiteSpace(rawValue))
            return null;

        return long.TryParse(rawValue, out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds)
            : null;
    }
}
