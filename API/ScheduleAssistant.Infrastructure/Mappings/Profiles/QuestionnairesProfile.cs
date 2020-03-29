using AutoMapper;
using ScheduleAssistant.Communication.Windows;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Infrastructure.Mappings.Profiles
{
    public class QuestionnairesProfile : Profile
    {
        public QuestionnairesProfile()
        {
            this.CreateMap<Window, WindowDto>();
            this.CreateMap<WindowDto, Window>();
        }
    }
}