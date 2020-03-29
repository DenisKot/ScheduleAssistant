using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}