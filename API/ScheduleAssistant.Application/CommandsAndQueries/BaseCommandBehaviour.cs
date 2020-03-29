using System;
using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Data.EntityFramework;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ScheduleAssistant.Application.CommandsAndQueries
{
    public abstract class BaseCommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<BaseCommandBehaviour<TRequest, TResponse>> logger;
        private readonly AppDbContext dbContext;

        protected BaseCommandBehaviour(ILogger<BaseCommandBehaviour<TRequest, TResponse>> logger, AppDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                await this.dbContext.Database.BeginTransactionAsync(cancellationToken);

                var response = await next();

                this.dbContext.Database.CommitTransaction();

                return response;
            }
            catch (Exception e)
            {
                this.logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}");
                this.logger.LogError(e.Message, e.StackTrace);

                this.dbContext.Database.RollbackTransaction();

                throw;
            }
        }
    }
}
