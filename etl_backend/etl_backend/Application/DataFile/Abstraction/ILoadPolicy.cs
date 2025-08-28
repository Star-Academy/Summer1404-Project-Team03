using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ILoadPolicy
{
    LoadMode Mode { get; }
    bool DropOnFailure { get; }   // if Replace and load fails, should we drop?
}