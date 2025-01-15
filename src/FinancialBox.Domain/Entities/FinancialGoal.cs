using FinancialBox.Domain.Enums;
using System.Transactions;

namespace FinancialBox.Domain.Entities;

public class FinancialGoal
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public DateTime? Deadline { get; set; }
    public decimal? IdealMonthlyContribution { get; set; }
    public FinancialGoalStatus Status { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public string CoverImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}
