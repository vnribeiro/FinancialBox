using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class User : AggregateRoot
{
    public Guid AccountId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    protected User() { }

    private User(Guid accountId, string firstName, string lastName)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
    }

    public static User Create(Guid accountId, string firstName, string lastName)
        => new(accountId, firstName, lastName);

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}
