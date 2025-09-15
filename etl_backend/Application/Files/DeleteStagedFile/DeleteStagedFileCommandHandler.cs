using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Commands;
using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Application.Services.Repositories.Abstractions;
using Domain.Enums;
using MediatR;

namespace Application.Files.Handlers;

public class DeleteStagedFileCommandHandler : IRequestHandler<DeleteStagedFileCommand>
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDeleteStagedFile _fileStagingService;

    public DeleteStagedFileCommandHandler(
        IStagedFileRepository stagedRepo,
        IDeleteStagedFile fileStagingService)
    {
        _stagedRepo = stagedRepo;
        _fileStagingService = fileStagingService;
    }

    public async Task Handle(DeleteStagedFileCommand request, CancellationToken ct)
    {
        var staged = await _stagedRepo.GetByIdAsync(request.StagedFileId, ct);
        if (staged is null)
            throw new NotFoundException("StagedFile", request.StagedFileId);
        if (staged.Stage == ProcessingStage.Loaded)
            throw new ConflictException("Cannot delete file after it has been loaded.");

        await _fileStagingService.DeleteAsync(staged.StoredFilePath, ct);
        await _stagedRepo.DeleteAsync(request.StagedFileId, ct);
    }
}