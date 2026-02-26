using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.FinancialGoals.Enums;

namespace FinancialBox.Domain.Features.FinancialGoals;

public class FinancialGoal : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public decimal TargetAmount { get; private set; }
    public DateTime? Deadline { get; private set; }
    public decimal? IdealMonthlyContribution { get; private set; }
    public FinancialGoalStatus Status { get; private set; }
    public string CoverImagePath { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }

    public Guid UserId { get; private set; }

    public ICollection<FinancialGoalTransactions> Transactions { get; private set; } = new List<FinancialGoalTransactions>();

    protected FinancialGoal() {}

    private FinancialGoal(string title, decimal targetAmount, Guid userId, DateTime? deadline = null)
    {
        Title = title;
        TargetAmount = targetAmount;
        Deadline = deadline;
        UserId = userId;
        Status = FinancialGoalStatus.InProgress;
        CoverImagePath = string.Empty;
        IdealMonthlyContribution = CalculateIdealMonthlyContribution();
    }

    public static FinancialGoal Create(string title, decimal targetAmount, Guid userId, DateTime? deadline = null)
        => new(title, targetAmount, userId, deadline);

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void UpdateCoverImage(string newPath)
    {
        CoverImagePath = newPath;
    }

    private decimal? CalculateIdealMonthlyContribution()
    {
        if (!Deadline.HasValue) return null;

        var remainingMonths = (Deadline.Value - DateTime.UtcNow).Days / 30m;
        return remainingMonths > 0 ? TargetAmount / remainingMonths : null;
    }
}

