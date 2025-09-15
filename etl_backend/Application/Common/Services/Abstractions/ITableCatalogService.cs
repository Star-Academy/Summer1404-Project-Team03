namespace Application.Abstractions;

using System.Collections.Generic;

public interface ITypeCatalogService
{
    IReadOnlyList<string> GetAllowedTypes();
    IReadOnlyDictionary<string, string> GetTypeAliases();
    bool TryNormalize(string input, out string normalized);
}