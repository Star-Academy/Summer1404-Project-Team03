using Application.Abstractions;
using Application.Enums;
using Application.Files.Commands;
using Application.Services.Abstractions;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;

public sealed class LoadPolicy : ILoadPolicy
{
    public LoadPolicy(LoadMode mode, bool dropOnFailure = false)
        => (Mode, DropOnFailure) = (mode, dropOnFailure);

    public LoadMode Mode { get; }
    public bool DropOnFailure { get; }
}
