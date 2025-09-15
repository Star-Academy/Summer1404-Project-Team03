using Application.Common.Exceptions;
using Application.Files.Commands;
using Application.Files.DeleteStagedFile;
using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TestProject1.Application;

public class DeleteStagedFileCommandHandlerTests
{
    private readonly IStagedFileRepository _stagedRepoMock;
    private readonly IDeleteStagedFile _fileStagingServiceMock;
    private readonly DeleteStagedFileCommandHandler _sut;

    public DeleteStagedFileCommandHandlerTests()
    {
        _stagedRepoMock = Substitute.For<IStagedFileRepository>();
        _fileStagingServiceMock = Substitute.For<IDeleteStagedFile>();
        _sut = new DeleteStagedFileCommandHandler(_stagedRepoMock, _fileStagingServiceMock);
    }
    [Fact]
    public async Task Handle_WhenStagedFileNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var command = new DeleteStagedFileCommand(fileId);
        _stagedRepoMock.GetByIdAsync(fileId, Arg.Any<CancellationToken>())
            .Returns((StagedFile?)null);

        // Act
        Func<Task> act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}