namespace WebApi.Files.RegisterAndLoad;

public class RegisterAndLoadResponse
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public List<RegisterSchemaColumnResponse> Columns { get; set; } = new();
    public StagedFileStatusResponse Staged { get; set; } = new();
    public LoadFileIntoTableResponse Load { get; set; } = new();
}