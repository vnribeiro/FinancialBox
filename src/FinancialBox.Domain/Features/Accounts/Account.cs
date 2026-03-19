using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.Accounts.ValueObjects;

namespace FinancialBox.Domain.Features.Accounts;

public class Account : AggregateRoot
{
    public Guid UserId { get; private set; }
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public bool IsEmailConfirmed { get; private set; }
    public ICollection<Role> Roles { get; private set; } = [];

    protected Account() { }

    private Account(Guid userId, Email email, Password password)
    {
        UserId = userId;
        Email = email;
        Password = password;
    }

    public static Account Create(Guid userId, Email email, Password password)
        => new(userId, email, password);

    public void UpdateEmail(Email newEmail) => Email = newEmail;
    public void UpdatePassword(Password newPassword) => Password = newPassword;
    public void ConfirmEmail() => IsEmailConfirmed = true;

    public void AddRole(Role role)
    {
        if (Roles.All(r => r.Id != role.Id))
            Roles.Add(role);
    }

    public void RemoveRole(Guid roleId)
    {
        var role = Roles.FirstOrDefault(r => r.Id == roleId);

        if (role is not null)
            Roles.Remove(role);
    }

    public bool HasRole(Guid roleId) => Roles.Any(r => r.Id == roleId);
}