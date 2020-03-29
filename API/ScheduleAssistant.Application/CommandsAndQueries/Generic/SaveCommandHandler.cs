using System;
using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Common;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain;
using MediatR;

namespace ScheduleAssistant.Application.CommandsAndQueries.Generic
{
    public class SaveCommandHandler<TDomain, TDto> : IRequestHandler<SaveCommand<TDto>, int>
        where TDomain : BaseEntity
        where TDto : BaseEntityDto
    {
        private readonly IRepository<TDomain> repository;
        private readonly IMappingService mappingService;

        public SaveCommandHandler(IRepository<TDomain> repository, IMappingService mappingService)
        {
            this.repository = repository;
            this.mappingService = mappingService;
        }

        public async Task<int> Handle(SaveCommand<TDto> request, CancellationToken cancellationToken)
        {
            if(request.EntityDto == null)
                throw new ArgumentException($"EntityDto of type {nameof(TDto)} is null in {nameof(SaveCommandHandler<TDomain, TDto>)}");

            TDomain entity;

            if (request.EntityDto.Id == 0)
            {
                entity = this.mappingService.Map<TDomain>(request.EntityDto);
                await this.repository.InsertAsync(entity);
            }
            else
            {
                entity = await this.repository.FirstOrDefaultAsync(request.EntityDto.Id);
                this.mappingService.Map<TDto, TDomain>(request.EntityDto, entity);
                await this.repository.UpdateAsync(entity);
            }
            
            await this.repository.SaveChangesAsync();

            return entity.Id;
        }
    }
}