namespace FinancialBox.Infrastructure.Email.Templates;

internal sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string PlainBody);
