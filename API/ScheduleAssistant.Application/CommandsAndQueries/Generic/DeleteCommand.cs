using ScheduleAssistant.Communication;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    public class DeleteCommand<TDto> : ICommand
        where TDto : BaseEntityDto
    {
        public int Id { get; set; }

        public DeleteCommand(int id)
        {
            this.Id = id;
        }
    }
}