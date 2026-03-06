using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.UnitTests.Domain.Users;

public class UserTests
{
    private static Email ValidEmail() => Email.Create("user@example.com").Data;
    private static Password ValidPassword() => Password.FromHash("hashed_pass");

    [Fact]
    public void Should_CreateUser_When_AllFieldsAreValid()
    {
        var email = ValidEmail();
        var password = ValidPassword();

        var user = User.Create("John", "Doe", email, password);

        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.False(user.IsEmailConfirmed);
        Assert.Empty(user.Roles);
    }

    [Fact]
    public void Should_ReturnFullName_When_GetFullNameCalled()
    {
        var user = User.Create("Jane", "Smith", ValidEmail(), ValidPassword());

        Assert.Equal("Jane Smith", user.GetFullName());
    }

    [Fact]
    public void Should_UpdateName_When_UpdateNameCalled()
    {
        var user = User.Create("Old", "Name", ValidEmail(), ValidPassword());

        user.UpdateName("New", "Name");

        Assert.Equal("New", user.FirstName);
        Assert.Equal("Name", user.LastName);
    }

    [Fact]
    public void Should_UpdateEmail_When_UpdateEmailCalled()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var newEmail = Email.Create("new@example.com").Data;

        user.UpdateEmail(newEmail);

        Assert.Equal("new@example.com", user.Email.Address);
    }

    [Fact]
    public void Should_UpdatePassword_When_UpdatePasswordCalled()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var newPassword = Password.FromHash("new_hash");

        user.UpdatePassword(newPassword);

        Assert.Equal(newPassword, user.Password);
    }

    [Fact]
    public void Should_ConfirmEmail_When_ConfirmEmailCalled()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());

        user.ConfirmEmail();

        Assert.True(user.IsEmailConfirmed);
    }

    [Fact]
    public void Should_AddRole_When_RoleNotAlreadyAssigned()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var role = new Role("Admin");

        user.AddRole(role);

        Assert.Single(user.Roles);
        Assert.Contains(user.Roles, r => r.Name == "Admin");
    }

    [Fact]
    public void Should_NotAddDuplicateRole_When_RoleAlreadyAssigned()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var role = new Role("Admin");
        user.AddRole(role);

        user.AddRole(role);

        Assert.Single(user.Roles);
    }

    [Fact]
    public void Should_ReturnTrue_When_UserHasRole()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var role = new Role("Admin");
        user.AddRole(role);

        Assert.True(user.HasRole(role.Id));
    }

    [Fact]
    public void Should_ReturnFalse_When_UserDoesNotHaveRole()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());

        Assert.False(user.HasRole(Guid.NewGuid()));
    }

    [Fact]
    public void Should_RemoveRole_When_RoleExists()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());
        var role = new Role("Admin");
        user.AddRole(role);

        user.RemoveRole(role.Id);

        Assert.Empty(user.Roles);
    }

    [Fact]
    public void Should_NotThrow_When_RemovingNonExistentRole()
    {
        var user = User.Create("John", "Doe", ValidEmail(), ValidPassword());

        var exception = Record.Exception(() => user.RemoveRole(Guid.NewGuid()));

        Assert.Null(exception);
    }
}
