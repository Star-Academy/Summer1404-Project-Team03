using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Handlers;
using Application.Files.Queries;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Tests.Files;

public class PreviewSchemaQueryHandlerTests
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IHeaderProvider _headerProvider;
    private readonly IColumnDefinitionBuilder _columnBuilder;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly PreviewSchemaQueryHandler _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public PreviewSchemaQueryHandlerTests()
    {
        _stagedRepo = Substitute.For<IStagedFileRepository>();
        _headerProvider = Substitute.For<IHeaderProvider>();
        _columnBuilder = Substitute.For<IColumnDefinitionBuilder>();
        _schemaRepo = Substitute.For<IDataTableSchemaRepository>();
        _sut = new PreviewSchemaQueryHandler(_stagedRepo, _headerProvider, _columnBuilder, _schemaRepo);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFound_When_StagedFile_Not_Exists()
    {
        var query = new PreviewSchemaQuery(1);
        _stagedRepo.GetByIdAsync(query.StagedFileId, _ct).Returns((StagedFile?)null);


        var act = async () => await _sut.Handle(query, _ct);


        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_Conflict_When_StagedFile_Is_Failed()
    {
        var staged = new StagedFile
        {
            Id = 1,
            OriginalFileName = "file.csv",
            StoredFilePath = "/files/file.csv",
            Status = ProcessingStatus.Failed
        };
        var query = new PreviewSchemaQuery(staged.Id);
        _stagedRepo.GetByIdAsync(query.StagedFileId, _ct).Returns(staged);


        var act = async () => await _sut.Handle(query, _ct);


        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_Should_Return_Columns_From_ExistingSchema_When_SchemaId_And_Columns_Exist()
    {
        var staged = new StagedFile
        {
            Id = 1,
            OriginalFileName = "file.csv",
            StoredFilePath = "/files/file.csv",
            SchemaId = 100
        };
        var existingSchema = new DataTableSchema
        {
            Id = 100,
            Columns = new List<DataTableColumn>
            {
                new() { ColumnName = "B", OrdinalPosition = 2 },
                new() { ColumnName = "A", OrdinalPosition = 1 }
            }
        };

        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _schemaRepo.GetByIdWithColumnsAsync(existingSchema.Id, _ct).Returns(existingSchema);

        var query = new PreviewSchemaQuery(staged.Id);


        var result = await _sut.Handle(query, _ct);


        result.StagedFileId.Should().Be(staged.Id);
        result.Columns.Should().HaveCount(2);
        result.Columns.Should().BeInAscendingOrder(c => c.OrdinalPosition);
        await _headerProvider.DidNotReceiveWithAnyArgs().GetAsync(default!, default);
        _columnBuilder.DidNotReceiveWithAnyArgs().Build(default!);
    }

    [Fact]
    public async Task Handle_Should_Throw_Unprocessable_When_HeaderProvider_Returns_Empty()
    {
        var staged = new StagedFile
        {
            Id = 1,
            OriginalFileName = "file.csv",
            StoredFilePath = "/files/file.csv",
            SchemaId = null
        };

        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headerProvider.GetAsync(staged, _ct).Returns(new List<string>());

        var query = new PreviewSchemaQuery(staged.Id);


        var act = async () => await _sut.Handle(query, _ct);


        await act.Should().ThrowAsync<UnprocessableEntityException>();
    }

    [Fact]
    public async Task Handle_Should_Use_HeaderProvider_And_ColumnBuilder_When_No_Schema()
    {
        var staged = new StagedFile
        {
            Id = 1,
            OriginalFileName = "file.csv",
            StoredFilePath = "/files/file.csv"
        };

        var headers = new List<string> { "Name", "Age" };
        var columnEntities = new List<DataTableColumn>
        {
            new() { ColumnName = "Name", OrdinalPosition = 1 },
            new() { ColumnName = "Age", OrdinalPosition = 2 }
        };

        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headerProvider.GetAsync(staged, _ct).Returns(headers);
        _columnBuilder.Build(headers).Returns(columnEntities);

        var query = new PreviewSchemaQuery(staged.Id);


        var result = await _sut.Handle(query, _ct);


        result.StagedFileId.Should().Be(staged.Id);
        result.Columns.Should().HaveCount(2);
        result.Columns.Select(c => c.ColumnName).Should().Contain(new[] { "Name", "Age" });
        await _schemaRepo.DidNotReceiveWithAnyArgs().GetByIdWithColumnsAsync(default, default);
    }
}