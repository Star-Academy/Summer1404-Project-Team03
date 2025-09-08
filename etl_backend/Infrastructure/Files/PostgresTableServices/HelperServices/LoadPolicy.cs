using Application.Abstractions;
using Application.Files.Commands;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;

public sealed class LoadPolicy : ILoadPolicy
{
    public LoadPolicy(LoadMode mode, bool dropOnFailure = false)
        => (Mode, DropOnFailure) = (mode, dropOnFailure);

    public LoadMode Mode { get; }
    public bool DropOnFailure { get; }
}
