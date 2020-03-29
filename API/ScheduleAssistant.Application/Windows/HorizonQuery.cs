using System;
using System.Collections.Generic;
using ScheduleAssistant.Application.CommandsAndQueries;
using ScheduleAssistant.Application.CommandsAndQueries.Generic;
using ScheduleAssistant.Communication.Windows;

namespace ScheduleAssistant.Application.Windows
{
    public class HorizonQuery : IQuery<IEnumerable<WindowDto>>
    {
        public int Horizon { get; set; }

        public DateTimeOffset CurrentDate { get; set; }
    }
}
