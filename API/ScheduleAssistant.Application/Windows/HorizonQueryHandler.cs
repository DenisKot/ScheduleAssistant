using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ScheduleAssistant.Common;
using ScheduleAssistant.Common.Exceptions;
using ScheduleAssistant.Communication.Windows;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Application.Windows
{
    public class HorizonQueryHandler : IRequestHandler<HorizonQuery, IEnumerable<WindowDto>>
    {
        private readonly IRepository<ExpressWindow> expressRepository;
        private readonly IRepository<UsualWindow> usualRepository;
        private readonly IMappingService mappingService;

        public HorizonQueryHandler(IRepository<ExpressWindow> expressRepository, 
            IMappingService mappingService,
            IRepository<UsualWindow> usualRepository)
        {
            this.expressRepository = expressRepository;
            this.mappingService = mappingService;
            this.usualRepository = usualRepository;
        }

        public async Task<IEnumerable<WindowDto>> Handle(HorizonQuery request, CancellationToken cancellationToken)
        {
            if(request.Horizon < 0)
                throw new ValidationException("Horizon days can't be negative");

            if (request.CurrentDate == DateTimeOffset.MinValue)
                throw new ValidationException("CurrentDate must be defined");

            var endTime = request.CurrentDate.AddDays(request.Horizon + 1);

            var result = await this.usualRepository.GetAllListAsync(
                w => 
                     w.Start > request.CurrentDate 
                     && w.Finish < endTime 
                     && w.Type == WindowType.UsualDelivery);

            // ToDo: clarify business requirements about what one express delivery should be choosed
            var expressWindow = await this.expressRepository.FirstOrDefaultAsync(w => w.Type == WindowType.ExpressDelivery);
            var expressWindowDto = this.mappingService.Map<WindowDto>(expressWindow);
            var resultDto = this.mappingService.Map<IEnumerable<WindowDto>>(result);
            
            var list = new List<WindowDto> { expressWindowDto };
            list.AddRange(resultDto);

            return list;
        }
    }
}
