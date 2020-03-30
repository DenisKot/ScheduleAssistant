using System;

namespace ScheduleAssistant.Domain.Windows
{
    public class ExpressWindow : BaseEntity
    {
        public ExpressWindow()
        {
            this.Type = WindowType.ExpressDelivery;
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public decimal Price { get; set; }

        public WindowType Type { get; protected set; }

        public bool Available { get; set; }
    }
}
