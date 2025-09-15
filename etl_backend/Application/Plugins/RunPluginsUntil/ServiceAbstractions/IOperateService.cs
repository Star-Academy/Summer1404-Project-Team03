using Domain.ValueObjects.PluginConfig;
using Microsoft.Spark.Sql;

namespace Application.Plugins.RunPluginsUntil.ServiceAbstractions;

public interface IOperateService
{
    DataFrame Apply(DataFrame df, PluginConfig cfg);
}