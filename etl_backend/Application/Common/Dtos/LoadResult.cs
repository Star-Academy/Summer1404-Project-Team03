namespace Application.Dtos;

public record LoadResult(
    long RowsInserted,
    double ElapsedMs
);