namespace Application.Dtos;

public sealed class RowPreviewDto
{
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int NextOffset { get; set; }
}
public sealed class RowCountDto
{
    public bool Exact { get; set; }
    public long Count { get; set; }
}