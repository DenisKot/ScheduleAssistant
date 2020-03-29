using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain;
using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    public class DeleteCommandHandler<TDomain, TDto> : IRequestHandler<DeleteCommand<TDto>>
        where TDomain : BaseEntity
        where TDto : BaseEntityDto
    {
        private readonly IRepository<TDomain> repository;

        public DeleteCommandHandler(IRepository<TDomain> repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(DeleteCommand<TDto> request, CancellationToken cancellationToken)
        {
            this.repository.Delete(request.Id);
            await this.repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}