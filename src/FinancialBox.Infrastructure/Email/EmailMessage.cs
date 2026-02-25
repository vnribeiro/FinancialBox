namespace FinancialBox.Infrastructure.Email;

internal sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string PlainBody);
