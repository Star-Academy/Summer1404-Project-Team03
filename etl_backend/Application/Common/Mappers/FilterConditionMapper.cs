using Application.Common.Exceptions;
using Application.Plugins.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Mappers;

public static class FilterConditionMapper
{
    public static FilterCondition ToDomain(this FilterConditionDto dto)
    {
        if (!Enum.TryParse<FilterOp>(dto.Op, true, out var op))
            throw new UnprocessableEntityException($"Invalid FilterOp: {dto.Op}");

        if (!Enum.TryParse<ValueTypeHint>(dto.TypeHint, true, out var typeHint))
            throw new UnprocessableEntityException($"Invalid ValueTypeHint: {dto.TypeHint}");

        return new FilterCondition
        {
            Column = dto.Column,
            Op = op,
            TypeHint = typeHint,
            Value = dto.Value,
            Value2 = dto.Value2,
            Values = dto.Values?.ToList()
        };
    }
}