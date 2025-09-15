using Domain.Entities;
using Microsoft.Spark.Sql;

namespace Infrastructure.Plugins.Abstractions;

public interface IConditionPredicateBuilder
{
    Column Build(Column column, FilterCondition condition);
}