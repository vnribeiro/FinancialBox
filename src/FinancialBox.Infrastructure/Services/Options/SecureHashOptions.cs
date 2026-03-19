namespace FinancialBox.Infrastructure.Services.Options;

internal sealed class SecureHashOptions
{
    public const string SectionName = "SecureHash";

    public int Iterations { get; set; }
    public int SaltSize { get; set; }
    public int SubkeySize { get; set; }
}