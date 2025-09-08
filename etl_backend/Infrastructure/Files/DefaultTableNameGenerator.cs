using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

public sealed class DefaultTableNameGenerator : ITableNameGenerator
{
    public string Generate(int stagedFileId, string originalFileName)
    {
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var slug = new string(baseName.ToLowerInvariant().Select(ch => char.IsLetterOrDigit(ch) ? ch : '_').ToArray());
        slug = string.Join("_", slug.Split('_', StringSplitOptions.RemoveEmptyEntries)).Trim('_');
        if (slug.Length > 40) slug = slug[..40];
        return $"csv_{stagedFileId}_{slug}";
    }
}