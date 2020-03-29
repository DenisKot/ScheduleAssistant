using System;
using System.Collections.Generic;
using ScheduleAssistant.Common.DependencyInjection;

namespace ScheduleAssistant.Common
{
    public interface IMappingService : ISingletonDependency
    {
        IEnumerable<Tuple<Type, Type>> GetMaps();
        
        TEntity Map<TEntity>(object value);

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}