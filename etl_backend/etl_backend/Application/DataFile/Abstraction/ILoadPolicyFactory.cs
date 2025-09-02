using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ILoadPolicyFactory { ILoadPolicy Create(LoadMode mode, bool dropOnFailure); }
