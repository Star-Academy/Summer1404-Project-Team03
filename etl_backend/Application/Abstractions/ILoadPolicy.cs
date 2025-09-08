using Application.Files.Commands;

namespace Application.Abstractions;

public interface ILoadPolicy
{
    LoadMode Mode { get; }
    bool DropOnFailure { get; }   // if Replace and load fails, should we drop?
}