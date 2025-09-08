namespace WebApi.Files;

public class LoadFileIntoTableRequest
{
    public int Id { get; set; }
    public string Mode { get; set; } = "Append"; // "Append" or "Truncate"
    public bool DropOnFailure { get; set; }
}