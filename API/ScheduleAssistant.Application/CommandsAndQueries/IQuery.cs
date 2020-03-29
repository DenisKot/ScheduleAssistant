using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}