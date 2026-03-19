namespace FinancialBox.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected BaseEntity(Guid id)
    {
        Id = id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty.", nameof(id));
        CreatedAt = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id == other.Id && GetType() == other.GetType();
    }

    public override int GetHashCode() => Id.GetHashCode();
}
