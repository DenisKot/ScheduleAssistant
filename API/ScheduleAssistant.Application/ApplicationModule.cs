using System;
using System.Collections.Generic;
using Autofac;
using MediatR;
using System.Reflection;
using ScheduleAssistant.Application.CommandsAndQueries;
using ScheduleAssistant.Application.CommandsAndQueries.Generic;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Domain;
using ScheduleAssistant.Infrastructure.Mappings;

namespace ScheduleAssistant.Application
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register future dependencies
            this.ConfigureMadiator(builder);
        }

        private void ConfigureMadiator(ContainerBuilder builder)
        {
            // Mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(ApplicationModule).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

            builder.RegisterGeneric(typeof(CommandResponseBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(CommandBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

            //builder.RegisterType<GetByIdQueryHandler<Category, CategoryDto>>().AsImplementedInterfaces().InstancePerDependency();
            
            var mappings = new MappingService().GetMaps();

            var baseEntityType = typeof(BaseEntity);
            var baseEntityDtoType = typeof(BaseEntityDto);

            foreach (var map in mappings)
            {
                var sourceType = map.Item1;
                var distType = map.Item2;

                if (baseEntityType.IsAssignableFrom(sourceType))
                {
                    // Register Queries
                    Type[] typeArgs = { sourceType, distType };
                    var getByIdType = this.BuildGenericType(typeof(GetByIdQueryHandler<,>), typeArgs);
                    var getAllType = this.BuildGenericType(typeof(GetAllQueryHandler<,>), typeArgs);

                    builder.RegisterType(getByIdType).AsImplementedInterfaces().InstancePerDependency();
                    builder.RegisterType(getAllType).AsImplementedInterfaces().InstancePerDependency();
                } 
                else if (baseEntityDtoType.IsAssignableFrom(sourceType))
                {
                    // Register Commands
                    Type[] typeArgs = { distType, sourceType };
                    var saveType = this.BuildGenericType(typeof(SaveCommandHandler<,>), typeArgs);
                    var deleteType = this.BuildGenericType(typeof(DeleteCommandHandler<,>), typeArgs);

                    builder.RegisterType(saveType).AsImplementedInterfaces().InstancePerDependency();
                    builder.RegisterType(deleteType).AsImplementedInterfaces().InstancePerDependency();
                }
            }
        }

        private Type BuildGenericType(Type baseType, Type[] typeArgs)
        {
            return baseType.MakeGenericType(typeArgs);
        }
    }
}