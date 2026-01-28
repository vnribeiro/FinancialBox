using FinancialBox.Domain.Common;
using FinancialBox.Domain.Users;

namespace FinancialBox.Domain.FinancialGoals;

public class FinancialGoal : BaseEntity, IAggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public decimal TargetAmount { get; private set; }
    public DateTime? Deadline { get; private set; }
    public decimal? IdealMonthlyContribution { get; private set; }
    public FinancialGoalStatus Status { get; private set; }
    public string CoverImagePath { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public ICollection<FinancialGoalTransactions> Transactions { get; private set; } = new List<FinancialGoalTransactions>();

    protected FinancialGoal() {}

    public FinancialGoal(string title, decimal targetAmount, Guid userId, DateTime? deadline = null)
    {
        Title = title;
        TargetAmount = targetAmount;
        Deadline = deadline;
        UserId = userId;
        Status = FinancialGoalStatus.InProgress;
        CoverImagePath = string.Empty;
        IdealMonthlyContribution = CalculateIdealMonthlyContribution();
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCoverImage(string newPath)
    {
        CoverImagePath = newPath;
        UpdatedAt = DateTime.UtcNow;
    }

    private decimal? CalculateIdealMonthlyContribution()
    {
        if (!Deadline.HasValue) return null;

        var remainingMonths = (Deadline.Value - DateTime.UtcNow).Days / 30m;
        return remainingMonths > 0 ? TargetAmount / remainingMonths : null;
    }
}
