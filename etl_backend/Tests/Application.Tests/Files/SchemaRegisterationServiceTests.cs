using Application.Abstractions;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Files;
using Infrastructure.Files.Abstractions;
using NSubstitute.ExceptionExtensions;

namespace Application.Tests.Files;

public class SchemaRegistrationServiceTests
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IHeaderProvider _headers;
    private readonly IColumnDefinitionBuilder _cols;
    private readonly ITableNameGenerator _names;
    private readonly IStagedFileStateService _state;
    private readonly IColumnTypeValidator _typeValidator;
    private readonly IColumnNameSanitizer _nameSanitizer;
    private readonly SchemaRegistrationService _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public SchemaRegistrationServiceTests()
    {
        _stagedRepo = Substitute.For<IStagedFileRepository>();
        _schemaRepo = Substitute.For<IDataTableSchemaRepository>();
        _headers = Substitute.For<IHeaderProvider>();
        _cols = Substitute.For<IColumnDefinitionBuilder>();
        _names = Substitute.For<ITableNameGenerator>();
        _state = Substitute.For<IStagedFileStateService>();
        _typeValidator = Substitute.For<IColumnTypeValidator>();
        _nameSanitizer = Substitute.For<IColumnNameSanitizer>();

        _sut = new SchemaRegistrationService(
            _stagedRepo, _schemaRepo, _headers, _cols, _names, _state, _typeValidator, _nameSanitizer);
    }

    private StagedFile CreateStagedFile(int id = 1, ProcessingStage stage = ProcessingStage.None)
        => new()
        {
            Id = id,
            OriginalFileName = "test.csv",
            StoredFilePath = "/files/test.csv",
            Stage = stage
        };

    [Fact]
    public async Task RegisterAsync_Should_Throw_When_StagedFile_Not_Found()
    {
        _stagedRepo.GetByIdAsync(1, _ct).Returns((StagedFile?)null);


        var act = async () => await _sut.RegisterAsync(1, null!, null!, _ct);


        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(ProcessingStage.TableCreated)]
    [InlineData(ProcessingStage.Loaded)]
    public async Task RegisterAsync_Should_Throw_When_Stage_Not_Allowed(ProcessingStage stage)
    {
        var staged = CreateStagedFile(stage: stage);
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);


        var act = async () => await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RegisterAsync_Should_Call_FailSchemaRegistration_When_HeaderProvider_Throws()
    {
        var staged = CreateStagedFile();
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Throws(new Exception("Header error"));


        var act = async () => await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        await act.Should().ThrowAsync<Exception>();
        await _state.Received(1).FailSchemaRegistrationAsync(staged, Arg.Any<string>(), _ct);
    }

    [Fact]
    public async Task RegisterAsync_Should_Call_FailSchemaRegistration_When_HeaderProvider_Returns_Empty()
    {
        var staged = CreateStagedFile();
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Returns(Array.Empty<string>());


        var act = async () => await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        await act.Should().ThrowAsync<InvalidOperationException>();
        await _state.Received(1).FailSchemaRegistrationAsync(staged, Arg.Any<string>(), _ct);
    }

    [Fact]
    public async Task RegisterAsync_Should_Call_FailSchemaDbWrite_When_TypeValidator_Throws()
    {
        var staged = CreateStagedFile();
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Returns(new[] { "A", "B" });

        var cols = new List<DataTableColumn>
        {
            new() { OrdinalPosition = 1, ColumnName = "A" },
            new() { OrdinalPosition = 2, ColumnName = "B" }
        };
        _cols.Build(Arg.Any<IReadOnlyList<string>>()).Returns(cols);

        _typeValidator.When(v => v.ValidateOrThrow(Arg.Any<IEnumerable<(int, string)>>()))
            .Do(_ => throw new Exception("Type validation failed"));


        var act = async () => await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        await act.Should().ThrowAsync<Exception>();
        await _state.Received(1).FailSchemaDbWriteAsync(staged, Arg.Any<string>(), _ct);
    }

    [Fact]
    public async Task RegisterAsync_Should_Create_New_Schema_When_No_SchemaId()
    {
        var staged = CreateStagedFile();
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Returns(new[] { "Name" });
        var cols = new List<DataTableColumn> { new() { OrdinalPosition = 1, ColumnName = "Name" } };
        _cols.Build(Arg.Any<IReadOnlyList<string>>()).Returns(cols);
        _typeValidator.TryNormalize(Arg.Any<string>(), out Arg.Any<string>())
            .Returns(x =>
            {
                x[1] = x[0];
                return true;
            });
        _names.Generate(staged.Id, staged.OriginalFileName).Returns("table_name");


        var (schema, returnedStaged) = await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        schema.TableName.Should().Be("table_name");
        await _schemaRepo.Received(1).AddAsync(schema, _ct);
        await _state.Received(1).MarkSchemaRegisteredAsync(staged, schema.Id, _ct);
        returnedStaged.Should().Be(staged);
    }

    [Fact]
    public async Task RegisterAsync_Should_Update_Existing_Schema_When_SchemaId_Present()
    {
        var staged = CreateStagedFile();
        staged.SchemaId = 99;

        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Returns(new[] { "Name" });

        var existingSchema = new DataTableSchema
            { Id = 99, Columns = new List<DataTableColumn> { new() { ColumnName = "Old" } } };
        _schemaRepo.GetByIdWithColumnsAsync(99, _ct).Returns(existingSchema);

        var cols = new List<DataTableColumn> { new() { OrdinalPosition = 1, ColumnName = "New" } };
        _cols.Build(Arg.Any<IReadOnlyList<string>>()).Returns(cols);
        _typeValidator.TryNormalize(Arg.Any<string>(), out Arg.Any<string>())
            .Returns(x =>
            {
                x[1] = x[0];
                return true;
            });


        var (schema, returnedStaged) = await _sut.RegisterAsync(staged.Id, null!, null!, _ct);


        schema.Columns.Should().ContainSingle(c => c.ColumnName == "New");
        await _schemaRepo.Received(1).UpdateAsync(schema, _ct);
        await _state.Received(1).MarkSchemaRegisteredAsync(staged, schema.Id, _ct);
        returnedStaged.Should().Be(staged);
    }

    [Fact]
    public async Task RegisterAsync_Should_Use_NameSanitizer_When_Provided()
    {
        var staged = CreateStagedFile();
        _stagedRepo.GetByIdAsync(staged.Id, _ct).Returns(staged);
        _headers.GetAsync(staged, _ct).Returns(new[] { "Name" });

        var cols = new List<DataTableColumn> { new() { OrdinalPosition = 1, ColumnName = "Name" } };
        _cols.Build(Arg.Any<IReadOnlyList<string>>()).Returns(cols);

        _typeValidator.TryNormalize(Arg.Any<string>(), out Arg.Any<string>())
            .Returns(x =>
            {
                x[1] = x[0];
                return true;
            });

        _nameSanitizer.Sanitize("Name", 1).Returns("SanitizedName");


        var (schema, _) = await _sut.RegisterAsync(staged.Id,
            new Dictionary<int, string> { { 1, "string" } },
            new Dictionary<int, string> { { 1, "Name" } },
            _ct);


        schema.Columns.Should().ContainSingle(c => c.ColumnName == "SanitizedName");
    }
}