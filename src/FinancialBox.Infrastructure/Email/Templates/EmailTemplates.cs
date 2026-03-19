using System.Collections.Concurrent;

namespace FinancialBox.Infrastructure.Email.Templates;

internal static class EmailTemplates
{
    private static readonly string ResourcePrefix = $"{typeof(EmailTemplates).Namespace}.Templates";
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static EmailMessage ConfirmationLink(string to, string token) => new(
        To: to,
        Subject: "FinancialBox – Confirm your email address",
        HtmlBody: $"<p>Click the link below to confirm your email address:</p><p><a href=\"/api/v1/auth/confirm-email?token={token}\">Confirm email</a></p><p>This link expires in 30 minutes.</p>",
        PlainBody: $"""
            Hello,

            Click the link below to confirm your email address:

                /api/v1/auth/confirm-email?token={token}

            This link expires in 30 minutes. If you did not create an account, you can safely ignore this email.

            — FinancialBox
            """);

    public static EmailMessage PasswordReset(string to, string token) => new(
        To: to,
        Subject: "FinancialBox – Password reset request",
        HtmlBody: LoadTemplate("PasswordReset.html").Replace("{{token}}", token),
        PlainBody: $"""
            Hello,

            We received a request to reset your password. Use the token below:

                {token}

            This token expires in 15 minutes. If you did not request a password reset, you can safely ignore this email.

            — FinancialBox
            """);

    private static string LoadTemplate(string fileName)
    {
        return Cache.GetOrAdd(fileName, fn =>
        {
            var resourceName = $"{ResourcePrefix}.{fn}";
            using var stream = typeof(EmailTemplates).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        });
    }
}
