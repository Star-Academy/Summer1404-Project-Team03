using Application.Files.Commands;
using Application.Files.StageManyFiles;
using Application.Files.StageManyFiles.ServiceAbstractions;
using Domain.Entities;
using NSubstitute.ExceptionExtensions;

namespace Application.Tests.Files;

public class StageManyFilesCommandHandlerTests
{
    private readonly IAddStageFileService _staging;
    private readonly StageManyFilesCommandHandler _handler;

    public StageManyFilesCommandHandlerTests()
    {
        _staging = Substitute.For<IAddStageFileService>();
        _handler = new StageManyFilesCommandHandler(_staging);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedItem_WhenFileIsEmpty()
    {
        var files = new List<FileUploadItem>
        {
            new("empty.csv", new MemoryStream())
        };
        var command = new StageManyFilesCommand(files, "subdir");


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Single(result);
        Assert.False(result[0].Success);
        Assert.Equal("empty.csv", result[0].FileName);

        await _staging.DidNotReceiveWithAnyArgs()
            .StageAsync(default!, default!, default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenFileIsValid()
    {
        var stagedFile = new StagedFile
        {
            Id = 1,
            OriginalFileName = "file.csv",
            StoredFilePath = "path/file.csv",
            FileSize = 123
        };

        _staging
            .StageAsync(Arg.Any<Stream>(), "file.csv", "subdir", Arg.Any<CancellationToken>())
            .Returns(stagedFile);

        var files = new List<FileUploadItem>
        {
            new("file.csv", new MemoryStream(new byte[] { 1, 2, 3 }))
        };
        var command = new StageManyFilesCommand(files, "subdir");


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Single(result);
        Assert.True(result[0].Success);
        Assert.NotNull(result[0].Data);
        Assert.Equal("file.csv", result[0].FileName);

        await _staging.Received(1)
            .StageAsync(Arg.Any<Stream>(), "file.csv", "subdir", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedItem_WhenStagingThrows()
    {
        _staging
            .StageAsync(Arg.Any<Stream>(), "bad.csv", "subdir", Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Something went wrong"));

        var files = new List<FileUploadItem>
        {
            new("bad.csv", new MemoryStream(new byte[] { 1, 2, 3 }))
        };
        var command = new StageManyFilesCommand(files, "subdir");


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Single(result);
        Assert.False(result[0].Success);
        Assert.Equal("bad.csv", result[0].FileName);
        Assert.Equal("Something went wrong", result[0].Error);

        await _staging.Received(1)
            .StageAsync(Arg.Any<Stream>(), "bad.csv", "subdir", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnMixedResults_ForMultipleFiles()
    {
        var stagedFile = new StagedFile
        {
            Id = 42,
            OriginalFileName = "good.csv",
            StoredFilePath = "uploads/good.csv",
            FileSize = 555
        };

        _staging
            .StageAsync(Arg.Any<Stream>(), "good.csv", "uploads", Arg.Any<CancellationToken>())
            .Returns(stagedFile);

        _staging
            .StageAsync(Arg.Any<Stream>(), "fail.csv", "uploads", Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Cannot process"));

        var files = new List<FileUploadItem>
        {
            new("empty.csv", new MemoryStream()),
            new("good.csv", new MemoryStream(new byte[] { 10, 20 })),
            new("fail.csv", new MemoryStream(new byte[] { 30 }))
        };

        var command = new StageManyFilesCommand(files, "uploads");


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(3, result.Count);

        Assert.False(result[0].Success);
        Assert.True(result[1].Success);
        Assert.False(result[2].Success);

        await _staging.Received(2)
            .StageAsync(Arg.Any<Stream>(), Arg.Any<string>(), "uploads", Arg.Any<CancellationToken>());
    }
}