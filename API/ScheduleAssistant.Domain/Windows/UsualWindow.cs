using System;

namespace ScheduleAssistant.Domain.Windows
{
    public class UsualWindow : ExpressWindow
    {
        public UsualWindow()
        {
            this.Type = WindowType.UsualDelivery;
        }

        public override string Name => $"{this.Start:t} - {this.Finish:t}";

        public override string Description => $"Доставка с {this.Start:t} до {this.Finish:t} часов";

        public DateTimeOffset Start { get; set; }

        public DateTimeOffset Finish { get; set; }
    }
}
