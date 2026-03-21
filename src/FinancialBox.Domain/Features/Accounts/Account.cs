using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.Accounts.Events;
using FinancialBox.Domain.Features.Accounts.ValueObjects;

namespace FinancialBox.Domain.Features.Accounts;

public class Account : AggregateRoot
{
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public bool IsEmailConfirmed { get; private set; }
    public ICollection<Role> Roles { get; private set;  } = [];
    public ICollection<EmailConfirmationToken> EmailConfirmationTokens { get; private set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; protected set; } = [];

    protected Account() { }

    private Account(Email email, Password password)
    {
        Email = email;
        Password = password;
    }

    public static Account Register(Email email, Password password, DateTime expiresAt)
    {
        var account = new Account(email, password);
        var confirmationToken = EmailConfirmationToken.Create(account.Id, expiresAt);
        account.AddEmailConfirmationToken(confirmationToken);
        account.AddDomainEvent(new UserRegisteredEvent(account.Id, email.Address, confirmationToken.Token));
        return account;
    }

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

    public void AddEmailConfirmationToken(EmailConfirmationToken token) => EmailConfirmationTokens.Add(token);

    public void AddRefreshToken(RefreshToken refreshToken) => RefreshTokens.Add(refreshToken);

    public void RevokeAllRefreshTokens(DateTime utcNow)
    {
        foreach (var token in RefreshTokens.Where(rt => rt.IsActive(utcNow)))
            token.Revoke();
    }
}