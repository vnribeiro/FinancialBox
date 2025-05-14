using FinancialBox.BuildingBlocks;
using FinancialBox.Domain.Enums;

namespace FinancialBox.Domain.Entities;

public class FinancialGoal : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public DateTime? Deadline { get; set; }
    public decimal? IdealMonthlyContribution { get; set; }
    public FinancialGoalStatus Status { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<FinancialGoalTransactions> Transactions { get; set; } = new List<FinancialGoalTransactions>();
}