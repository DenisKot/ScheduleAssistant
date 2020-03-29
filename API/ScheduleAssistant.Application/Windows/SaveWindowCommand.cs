using ScheduleAssistant.Application.CommandsAndQueries;
using ScheduleAssistant.Communication.Windows;

namespace ScheduleAssistant.Application.Windows
{
    public class SaveWindowCommand : ICommand<int>
    {
        public WindowDto EntityDto { get; set; }
    }
}
