using Domain.Entities;
using Microsoft.Spark.Sql;
namespace Application.Plugins.RunPluginsUntil.ServiceAbstractions;

public interface IFilterService : IOperateService
{
    DataFrame Apply(DataFrame df, FilterConfig cfg);
}