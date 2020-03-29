using Autofac;
using ScheduleAssistant.Data.Repository;

namespace ScheduleAssistant.Data
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}