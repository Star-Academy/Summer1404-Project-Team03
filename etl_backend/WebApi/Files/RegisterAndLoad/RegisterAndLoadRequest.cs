namespace WebApi.Files.RegisterAndLoad;

public class RegisterAndLoadRequest
{
    public int Id { get; set; }
    public List<RegisterSchemaColumnItem> Columns { get; set; } = new();
    public string LoadMode { get; set; } = "Append";
    public bool DropOnFailure { get; set; }
}
