using Application.Enums;
using Application.Services.Abstractions;

namespace Application.Abstractions;

public interface ILoadPolicyFactory { ILoadPolicy Create(LoadMode mode, bool dropOnFailure); }
