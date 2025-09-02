using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Services;

public sealed class LoadPolicyFactory : ILoadPolicyFactory { public ILoadPolicy Create(LoadMode m, bool d) => new LoadPolicy(m, d); }
