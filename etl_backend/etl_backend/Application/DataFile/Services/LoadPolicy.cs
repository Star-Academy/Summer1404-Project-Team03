using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Services;

public sealed class LoadPolicy : ILoadPolicy
{
    public LoadPolicy(LoadMode mode, bool dropOnFailure = false)
        => (Mode, DropOnFailure) = (mode, dropOnFailure);

    public LoadMode Mode { get; }
    public bool DropOnFailure { get; }
}
