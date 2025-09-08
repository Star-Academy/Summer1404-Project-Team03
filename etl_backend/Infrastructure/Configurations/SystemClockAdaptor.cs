using Domain.AccessControl.Ports;

namespace Infrastructure.Configurations;

public sealed class SystemClock : IClock { public DateTime UtcNow => DateTime.UtcNow; }
