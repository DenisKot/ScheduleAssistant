using ScheduleAssistant.Data.EntityFramework;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ScheduleAssistant.Application.CommandsAndQueries
{
    public class CommandResponseBehaviour<TRequest, TResponse> : BaseCommandBehaviour<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public CommandResponseBehaviour(ILogger<BaseCommandBehaviour<TRequest, TResponse>> logger, AppDbContext dbContext) : base(logger, dbContext)
        {
        }
    }
}
