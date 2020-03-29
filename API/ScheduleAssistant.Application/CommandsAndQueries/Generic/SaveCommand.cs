using ScheduleAssistant.Communication;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    public class SaveCommand<TDto> : ICommand<int>
        where TDto : BaseEntityDto
    {
        public TDto EntityDto { get; set; }

        public SaveCommand(TDto entityDto)
        {
            this.EntityDto = entityDto;
        }
    }
}