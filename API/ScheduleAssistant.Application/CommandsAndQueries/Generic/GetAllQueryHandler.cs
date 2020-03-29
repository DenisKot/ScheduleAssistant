using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Common;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain;
using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    internal class GetAllQueryHandler<TDomain, TDto> : IRequestHandler<GetAllQuery<IEnumerable<TDto>>, IEnumerable<TDto>>
        where TDomain : BaseEntity
    {
        private readonly IRepository<TDomain> repository;
        private readonly IMappingService mappingService;

        public GetAllQueryHandler(IRepository<TDomain> repository, IMappingService mappingService)
        {
            this.repository = repository;
            this.mappingService = mappingService;
        }

        public async Task<IEnumerable<TDto>> Handle(GetAllQuery<IEnumerable<TDto>> request, CancellationToken cancellationToken)
        {
            var list = await this.repository.GetAllListAsync();
            return this.mappingService.Map<List<TDto>>(list);
        }
    }
}