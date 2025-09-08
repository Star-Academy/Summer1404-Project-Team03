using Application.Files.Commands;
using Application.Files.Services;

namespace Application.Abstractions;
public interface IRegisterAndLoadService
{
    Task<RegisterAndLoadResult> ExecuteAsync(
        int stagedFileId,
        Dictionary<int, string> columnTypeMap,
        LoadMode mode = LoadMode.Append,
        bool dropOnFailure = false,
        CancellationToken ct = default);
}