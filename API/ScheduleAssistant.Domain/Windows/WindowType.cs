namespace ScheduleAssistant.Domain.Windows
{
    public enum WindowType : short
    {
        None = 0,

        /// <summary>
        /// Обычная доставка
        /// </summary>
        UsualDelivery = 1,

        /// <summary>
        /// Срочная доставка
        /// </summary>
        ExpressDelivery = 2
    }
}
