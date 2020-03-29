using ScheduleAssistant.Communication;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    public class GetByIdQuery<TDto> : IQuery<TDto>
    {
        public int Id { get; set; }

        public GetByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}