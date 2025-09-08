using MediatR;

namespace Application.Files.Commands;

public record DeleteStagedFileCommand(int StagedFileId) : IRequest;