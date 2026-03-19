using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class User : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    protected User() { }

    private User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static User Create(string firstName, string lastName)
        => new(firstName, lastName);

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}
