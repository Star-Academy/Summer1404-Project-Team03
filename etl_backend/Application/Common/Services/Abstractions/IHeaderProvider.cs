using Domain.Entities;

namespace Application.Abstractions;

public interface IHeaderProvider
{
    Task<IReadOnlyList<string>> GetAsync(StagedFile staged, CancellationToken ct = default);
}