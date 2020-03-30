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
        private readonly IRepository<Window> repository;
        private readonly IMappingService mappingService;

        public HorizonQueryHandler(IRepository<Window> repository, IMappingService mappingService)
        {
            this.repository = repository;
            this.mappingService = mappingService;
        }

        public async Task<IEnumerable<WindowDto>> Handle(HorizonQuery request, CancellationToken cancellationToken)
        {
            if(request.Horizon < 0)
                throw new ValidationException("Horizon days can't be negative");

            if (request.CurrentDate == DateTimeOffset.MinValue)
                throw new ValidationException("CurrentDate must be defined");

            var endTime = request.CurrentDate.AddDays(request.Horizon + 1);

            var result = await this.repository.GetAllListAsync(w => 
                     w.Start > request.CurrentDate 
                     && w.Finish < endTime 
                     && w.Type == WindowType.UsualDelivery);

            // ToDo: clarify business requirements about what one express delivery should be chose
            var expressWindow = await this.repository.FirstOrDefaultAsync(w =>
                w.Start < request.CurrentDate
                && w.Finish > request.CurrentDate
                && w.Type == WindowType.ExpressDelivery);

            var list = new List<Window>();
            
            if(expressWindow != null)
                list.Add(expressWindow);
            
            list.AddRange(result);

            return this.mappingService.Map<IEnumerable<WindowDto>>(list);
        }
    }
}
