using Application.Enums;
using Application.Services.Abstractions;

namespace Infrastructure.Files.Abstractions;

public interface ILoadPolicyFactory { ILoadPolicy Create(LoadMode mode, bool dropOnFailure); }
