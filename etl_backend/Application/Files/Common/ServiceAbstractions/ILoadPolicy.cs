using Application.Enums;

namespace Application.Common.Services.Abstractions;

public interface ILoadPolicy
{
    LoadMode Mode { get; }
    bool DropOnFailure { get; }   // if Replace and load fails, should we drop?
}