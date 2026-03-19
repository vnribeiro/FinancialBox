using FinancialBox.Domain.Features.FinancialGoals;
using FinancialBox.Domain.Features.FinancialGoals.Enums;

namespace FinancialBox.UnitTests.Domain.FinancialGoals;

public class FinancialGoalTests
{
    private static readonly Guid UserId = Guid.NewGuid();

    [Fact]
    public void Should_CreateGoal_With_InProgressStatus_And_NoIdealContribution_When_NoDeadline()
    {
        var goal = FinancialGoal.Create("Buy a car", 50000m, UserId);

        Assert.Equal("Buy a car", goal.Title);
        Assert.Equal(50000m, goal.TargetAmount);
        Assert.Equal(UserId, goal.UserId);
        Assert.Equal(FinancialGoalStatus.InProgress, goal.Status);
        Assert.Null(goal.Deadline);
        Assert.Null(goal.IdealMonthlyContribution);
        Assert.False(goal.IsDeleted);
    }

    [Fact]
    public void Should_CalculateIdealMonthlyContribution_When_DeadlineIsInFuture()
    {
        var deadline = DateTime.UtcNow.AddDays(300); // ~10 months
        var goal = FinancialGoal.Create("Emergency fund", 12000m, UserId, deadline);

        Assert.NotNull(goal.IdealMonthlyContribution);
        Assert.True(goal.IdealMonthlyContribution > 0);
    }

    [Fact]
    public void Should_NotCalculateIdealMonthlyContribution_When_DeadlineIsInPast()
    {
        var deadline = DateTime.UtcNow.AddDays(-10);
        var goal = FinancialGoal.Create("Old goal", 5000m, UserId, deadline);

        Assert.Null(goal.IdealMonthlyContribution);
    }

    [Fact]
    public void Should_UpdateTitle_When_UpdateTitleCalled()
    {
        var goal = FinancialGoal.Create("Old Title", 1000m, UserId);

        goal.UpdateTitle("New Title");

        Assert.Equal("New Title", goal.Title);
    }

    [Fact]
    public void Should_MarkAsDeleted_When_MarkAsDeletedCalled()
    {
        var goal = FinancialGoal.Create("Some Goal", 1000m, UserId);

        goal.MarkAsDeleted();

        Assert.True(goal.IsDeleted);
    }

    [Fact]
    public void Should_UpdateCoverImage_When_UpdateCoverImageCalled()
    {
        var goal = FinancialGoal.Create("Some Goal", 1000m, UserId);

        goal.UpdateCoverImage("images/cover.png");

        Assert.Equal("images/cover.png", goal.CoverImagePath);
    }

    [Fact]
    public void Should_HaveEmptyTransactions_When_Created()
    {
        var goal = FinancialGoal.Create("Some Goal", 1000m, UserId);

        Assert.Empty(goal.Transactions);
    }
}
