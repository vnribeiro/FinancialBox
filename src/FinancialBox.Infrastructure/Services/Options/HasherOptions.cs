namespace FinancialBox.Infrastructure.Services.Options;

internal sealed class HasherOptions
{
    public const string SectionName = "Hasher";

    public int Iterations { get; set; }
    public int SaltSize { get; set; }
    public int SubkeySize { get; set; }
}