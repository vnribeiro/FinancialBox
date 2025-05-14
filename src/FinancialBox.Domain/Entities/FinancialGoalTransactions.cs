using FinancialBox.BuildingBlocks;
using FinancialBox.Domain.Enums;

namespace FinancialBox.Domain.Entities;

public class FinancialGoalTransactions : BaseEntity
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime TransactionDate { get; set; }
    public bool IsDeleted { get; set; }

    public Guid FinancialGoalId { get; set; }
    public FinancialGoal FinancialGoal { get; set; } = null!;
}