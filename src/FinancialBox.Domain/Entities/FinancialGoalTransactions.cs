using FinancialBox.Domain.Enums;

namespace FinancialBox.Domain.Entities;

public class FinancialGoalTransactions
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}