using Application.Abstractions;
using Application.Files.Commands;

namespace Infrastructure.Files.Abstractions;

public interface ILoadPolicyFactory { ILoadPolicy Create(LoadMode mode, bool dropOnFailure); }
