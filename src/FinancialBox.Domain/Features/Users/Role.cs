using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class Role : BaseEntity, IAggregateRoot
{  
    public static string DefaultName => "User";

    public string Name { get; private set; } = string.Empty;
    public ICollection<User> Users { get; private set; } = [];

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
