using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class Role : AggregateRoot
{  
    public static string DefaultName => "User";

    public string Name { get; private set; } = string.Empty;

    protected Role() {}

    public Role(string name)
    {
        Name = name;
    }

    public void Rename(string name)
    {
        Name = name;
    }
}
