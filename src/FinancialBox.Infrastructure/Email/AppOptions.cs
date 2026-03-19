namespace FinancialBox.Infrastructure.Email;

internal sealed class AppOptions
{
    public const string SectionName = "App";

    public string BaseUrl { get; set; } = string.Empty;
}
