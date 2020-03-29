using System.Runtime.Serialization;

namespace ScheduleAssistant.Communication.Windows
{
    public enum WindowTypeDto : short
    {
        /// <summary>
        /// Обычная доставка
        /// </summary>
        [EnumMember(Value = "usual")]
        UsualDelivery = 1,

        /// <summary>
        /// Срочная доставка
        /// </summary>
        [EnumMember(Value = "express")]
        ExpressDelivery = 2
    }
}
