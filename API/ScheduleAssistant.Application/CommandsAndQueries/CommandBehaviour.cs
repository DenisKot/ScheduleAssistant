using ScheduleAssistant.Data.EntityFramework;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ScheduleAssistant.Application.CommandsAndQueries
{

    public class CommandBehaviour<TRequest, TResponse> : BaseCommandBehaviour<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
    {
        public CommandBehaviour(ILogger<BaseCommandBehaviour<TRequest, TResponse>> logger, AppDbContext dbContext) : base(logger, dbContext)
        {
        }
    }
}
