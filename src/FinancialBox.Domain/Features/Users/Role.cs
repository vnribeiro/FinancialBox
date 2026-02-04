using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

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
