namespace WebApi.Files;

public class StageManyFilesRequest
{
    public List<IFormFile>? Files { get; set; }
    public string? Subdir { get; set; }
}