using Application.Common.Exceptions;
using Application.Files.Commands;
using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Application.Files.Handlers;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Tests.Files;

public class DeleteStagedFileCommandHandlerTests
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDeleteStagedFile _fileStagingService;
    private readonly DeleteStagedFileCommandHandler _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public DeleteStagedFileCommandHandlerTests()
    {
        _stagedRepo = Substitute.For<IStagedFileRepository>();
        _fileStagingService = Substitute.For<IDeleteStagedFile>();
        _sut = new DeleteStagedFileCommandHandler(_stagedRepo, _fileStagingService);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFound_When_StagedFile_Does_Not_Exist()
    {
        var command = new DeleteStagedFileCommand(1);
        _stagedRepo.GetByIdAsync(command.StagedFileId, _ct)
            .Returns((StagedFile?)null);

        var act = async () => await _sut.Handle(command, _ct);


        await act.Should().ThrowAsync<NotFoundException>();
        await _fileStagingService.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
        await _stagedRepo.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
    }

    [Fact]
    public async Task Handle_Should_Throw_Conflict_When_StagedFile_Is_Loaded()
    {
        var staged = new StagedFile
        {
            Id = 1,
            StoredFilePath = "/files/test.txt",
            OriginalFileName = "test.txt",
            Stage = ProcessingStage.Loaded
        };
        var command = new DeleteStagedFileCommand(staged.Id);
        _stagedRepo.GetByIdAsync(command.StagedFileId, _ct).Returns(staged);


        var act = async () => await _sut.Handle(command, _ct);


        await act.Should().ThrowAsync<ConflictException>();
        await _fileStagingService.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
        await _stagedRepo.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
    }

    [Fact]
    public async Task Handle_Should_Delete_File_And_Record_When_Valid()
    {
        var staged = new StagedFile
        {
            Id = 1,
            OriginalFileName = "test.txt",
            StoredFilePath = "/files/test.txt",
            Stage = ProcessingStage.Uploaded
        };
        var command = new DeleteStagedFileCommand(staged.Id);
        _stagedRepo.GetByIdAsync(command.StagedFileId, _ct).Returns(staged);


        await _sut.Handle(command, _ct);


        await _fileStagingService.Received(1).DeleteAsync(staged.StoredFilePath, _ct);
        await _stagedRepo.Received(1).DeleteAsync(staged.Id, _ct);
    }
}