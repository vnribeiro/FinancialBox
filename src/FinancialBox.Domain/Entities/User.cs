﻿using FinancialBox.BuildingBlocks.DomainObjects;

namespace FinancialBox.Domain.Entities;

public class User : BaseEntity, IAggregateRoot
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<FinancialGoal> FinancialGoals { get; set; } = new List<FinancialGoal>();
}
