namespace etl_backend.Application.DataFile.Abstraction;

public interface ITypeMapper
{
    string ToProviderType(string logicalType);
}