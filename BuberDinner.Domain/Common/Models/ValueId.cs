namespace BuberDinner.Domain.Common.Models;

public abstract class ValueId<TId> : ValueObject
{
    public abstract TId Value { get; protected set; }
}