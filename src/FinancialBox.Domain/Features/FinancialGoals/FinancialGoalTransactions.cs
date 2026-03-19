using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.FinancialGoals.Enums;

namespace FinancialBox.Domain.Features.FinancialGoals;

public class FinancialGoalTransactions : BaseEntity
{
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public bool IsDeleted { get; private set; }

    public Guid FinancialGoalId { get; private set; }
    public FinancialGoals.FinancialGoal FinancialGoal { get; private set; } = null!;

    protected FinancialGoalTransactions() {}

    public FinancialGoalTransactions(decimal amount, TransactionType type, Guid financialGoalId, DateTime? transactionDate = null)
    {
        Amount = amount;
        Type = type;
        FinancialGoalId = financialGoalId;
        TransactionDate = transactionDate ?? DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}

