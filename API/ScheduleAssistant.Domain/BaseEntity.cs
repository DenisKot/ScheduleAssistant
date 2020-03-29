using System.ComponentModel.DataAnnotations;

namespace ScheduleAssistant.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}