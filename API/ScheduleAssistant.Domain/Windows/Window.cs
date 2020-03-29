using System;

namespace ScheduleAssistant.Domain.Windows
{
    public class Window : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Start { get; set; }

        public DateTimeOffset Finish { get; set; }

        public decimal Price { get; set; }

        public WindowType Type { get; set; }

        public bool Available { get; set; }
    }
}
