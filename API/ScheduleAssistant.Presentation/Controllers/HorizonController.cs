using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleAssistant.Application.CommandsAndQueries.Generic;
using ScheduleAssistant.Communication.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScheduleAssistant.Application.Windows;

namespace ScheduleAssistant.Presentation.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/horizon")]
    [ApiController]
    public class HorizonController : Controller
    {
        private readonly IMediator mediator;

        public HorizonController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Эндпоинт получения окон на указанное время и горизонт (в днях)
        /// </summary>
        /// <param name="horizon">Горизонт в днях, на который рассчитать доступные окна</param>
        /// <param name="currentDate">Время с часовым поясом (2020-04-02T13:59:30.977+03:00), относительно которого выполнить проверку доступных окон</param>
        /// <typeparam  name="currentDate">2020-04-02T13:59:30.977+03:00</typeparam >
        /// <returns>Список доступных окон</returns>
        [HttpGet]
        public async Task<IEnumerable<WindowDto>> Get(int horizon, DateTimeOffset currentDate)
        {
            return await this.mediator.Send(new HorizonQuery { Horizon = horizon, CurrentDate = currentDate });
        }
    }
}
