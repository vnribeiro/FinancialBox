using System.Collections.Concurrent;

namespace FinancialBox.Infrastructure.Email;

internal static class EmailTemplates
{
    private static readonly string ResourcePrefix = $"{typeof(EmailTemplates).Namespace}.Templates";
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static EmailMessage VerificationCode(string to, string code) => new(
        To: to,
        Subject: "FinancialBox – Email verification code",
        HtmlBody: LoadTemplate("VerificationCode.html").Replace("{{code}}", code),
        PlainBody: $"""
            Hello,

            Use the code below to verify your email address:

                {code}

            This code expires in 15 minutes. If you did not request this, you can safely ignore this email.

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
