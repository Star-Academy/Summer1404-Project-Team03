namespace Application.Plugins.Dtos;


public record FilterConditionDto(
    string Column,
    string Op,          
    string TypeHint,     
    string? Value,
    string? Value2,
    IEnumerable<string>? Values
);