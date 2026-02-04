using FinancialBox.Domain.Common;
using System.Collections.Generic;

namespace FinancialBox.Domain.Features.Users;

public sealed class Role : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; private set; } = [];

    protected Role() {}

    public Role(string name)
    {
        Name = name;
    }
}
