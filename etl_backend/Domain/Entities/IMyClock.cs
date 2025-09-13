namespace Domain.Entities;

public interface IClock
{
    DateTime UtcNow { get; }
}