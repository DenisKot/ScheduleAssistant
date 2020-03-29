using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Common;
using ScheduleAssistant.Data.Exceptions;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain;
using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    internal class GetByIdQueryHandler<TDomain, TDto> : IRequestHandler<GetByIdQuery<TDto>, TDto>
        where TDomain : BaseEntity
    {
        private readonly IRepository<TDomain> repository;
        private readonly IMappingService mappingService;

        public GetByIdQueryHandler(IRepository<TDomain> repository, IMappingService mappingService)
        {
            this.repository = repository;
            this.mappingService = mappingService;
        }

        public async Task<TDto> Handle(GetByIdQuery<TDto> request, CancellationToken cancellationToken)
        {
            var entity = await this.repository.FirstOrDefaultAsync(request.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TDomain), request.Id);
            }

            return this.mappingService.Map<TDto>(entity);
        }
    }
}