using Autofac;
using AutoMapper;
using ScheduleAssistant.Infrastructure.Mappings.Profiles;

namespace ScheduleAssistant.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register dependencies
        }

        internal static Profile[] GetAutoMapperProfiles()
        {
            return new Profile[] {new QuestionnairesProfile()};
        }
    }
}