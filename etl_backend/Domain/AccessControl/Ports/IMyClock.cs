namespace Domain.AccessControl.Ports;

public interface IClock
{
    DateTime UtcNow { get; }
}