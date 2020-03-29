using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ScheduleAssistant.Common;

namespace ScheduleAssistant.Infrastructure.Mappings
{
    public class MappingService : IMappingService
    {
        private readonly Mapper mapper;
        public static Mapper Mapper;

        public MappingService()
        {
            var profiles = InfrastructureModule.GetAutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            
            this.mapper = new Mapper(configuration);
            Mapper = this.mapper;
        }

        public IEnumerable<Tuple<Type, Type>> GetMaps()
        {
            return this.mapper.ConfigurationProvider.GetAllTypeMaps().Select(x => new Tuple<Type, Type>(x.SourceType, x.DestinationType));
        }

        public TEntity Map<TEntity>(object value)
        {
            return this.mapper.Map<TEntity>(value);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return this.mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}