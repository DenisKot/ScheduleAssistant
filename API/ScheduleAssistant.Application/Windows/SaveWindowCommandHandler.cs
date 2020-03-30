using MediatR;
using ScheduleAssistant.Common;
using ScheduleAssistant.Communication.Windows;
using ScheduleAssistant.Data.Repository;
using System.Threading;
using System.Threading.Tasks;
using ScheduleAssistant.Common.Exceptions;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Application.Windows
{
    public class SaveWindowCommandHandler : IRequestHandler<SaveWindowCommand, int>
    {
        private readonly IRepository<ExpressWindow> repository;
        private readonly IMappingService mappingService;

        public SaveWindowCommandHandler(IRepository<ExpressWindow> repository, IMappingService mappingService)
        {
            this.repository = repository;
            this.mappingService = mappingService;
        }

        public async Task<int> Handle(SaveWindowCommand request, CancellationToken cancellationToken)
        {
            this.Validate(request.EntityDto);

            ExpressWindow entity;

            if (request.EntityDto.Id == 0)
            {
                entity = this.mappingService.Map<ExpressWindow>(request.EntityDto);
                await this.repository.InsertAsync(entity);
            }
            else
            {
                entity = await this.repository.FirstOrDefaultAsync(request.EntityDto.Id);
                this.mappingService.Map<WindowDto, ExpressWindow>(request.EntityDto, entity);
                await this.repository.UpdateAsync(entity);
            }

            await this.repository.SaveChangesAsync();

            return entity.Id;
        }

        private void Validate(WindowDto dto)
        {
            if (dto == null)
                throw new ValidationException($"EntityDto of type {nameof(WindowDto)} is null in {nameof(SaveWindowCommandHandler)}");

            if(dto.Start.Date != dto.Finish.Date)
                throw new ValidationException($"Start and finish dates should be the same");

            // ToDo: check if overlaps with existing shedule
        }
    }
}
