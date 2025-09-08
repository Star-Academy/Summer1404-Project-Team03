using Application.Files.Commands;

namespace Application.Abstractions;

public interface ILoadPolicyFactory { ILoadPolicy Create(LoadMode mode, bool dropOnFailure); }
