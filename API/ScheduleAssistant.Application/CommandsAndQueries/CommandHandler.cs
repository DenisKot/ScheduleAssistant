using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries
{
    public abstract class CommandHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        async Task<Unit> IRequestHandler<TRequest, Unit>.Handle(TRequest request, CancellationToken cancellationToken)
        {
            await this.Handle(request, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
        

        protected abstract Task Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}