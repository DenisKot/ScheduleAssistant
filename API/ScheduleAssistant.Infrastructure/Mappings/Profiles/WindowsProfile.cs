using System;
using AutoMapper;
using ScheduleAssistant.Common;
using ScheduleAssistant.Communication.Windows;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Infrastructure.Mappings.Profiles
{
    public class WindowsProfile : Profile
    {
        public WindowsProfile()
        {
            this.CreateMap<ExpressWindow, WindowDto>()
                .ForMember(
                dist => dist.Start,
                            opt => opt.MapFrom(src => DateTimeOffset.UtcNow)
                )
                .ForMember(
                    dist => dist.Finish,
                    opt => opt.MapFrom(src => new DateTimeOffset(DateTime.Today.AddDays(1).AddSeconds(-1), Constants.TimeZoneOffset))
                );
            this.CreateMap<WindowDto, ExpressWindow>();

            this.CreateMap<UsualWindow, WindowDto>();
                //.ForMember(
                //    dist => dist.Start,
                //    opt => opt.MapFrom(src => src.Start)
                //)
                //.ForMember(
                //    dist => dist.Finish,
                //    opt => opt.MapFrom(src => src.Finish)
                //);
            this.CreateMap<WindowDto, UsualWindow>();
        }
    }
}