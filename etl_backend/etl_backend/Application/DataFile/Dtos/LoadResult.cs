namespace etl_backend.Application.DataFile.Dtos;

public sealed record LoadResult(long RowsInserted, TimeSpan Duration);
