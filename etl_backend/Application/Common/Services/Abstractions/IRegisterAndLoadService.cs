using Application.Dtos;
using Application.Enums;
using Application.Files.Commands;

namespace Application.Abstractions;
public interface IRegisterAndLoadService
{
    Task<RegisterAndLoadResult> ExecuteAsync(
        int stagedFileId,
        Dictionary<int, string> columnTypeMap,
        Dictionary<int, string> columnNameMap,
        LoadMode mode = LoadMode.Append,
        bool dropOnFailure = false,
        CancellationToken ct = default);
}