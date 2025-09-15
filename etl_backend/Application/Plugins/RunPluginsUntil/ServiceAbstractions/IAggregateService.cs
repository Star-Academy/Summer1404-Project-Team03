using Domain.Entities;
using Microsoft.Spark.Sql;

namespace Application.Plugins.RunPluginsUntil.ServiceAbstractions;

public interface IAggregateService : IOperateService
{
    DataFrame Apply(DataFrame df, AggregateConfig cfg);
}