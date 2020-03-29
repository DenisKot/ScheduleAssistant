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
    [Route("api/v1/window")]
    [ApiController]
    public class WindowController : Controller
    {
        private readonly IMediator mediator;

        public WindowController(IMediator mediator)
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
        [Route("horizon")]
        public async Task<IEnumerable<WindowDto>> GetHorizon(int horizon, DateTimeOffset currentDate)
        {
            return await this.mediator.Send(new HorizonQuery { Horizon = horizon, CurrentDate = currentDate });
        }

        /// <summary>
        /// Получить все окна
        /// </summary>
        /// <returns>Список всех окон</returns>
        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<WindowDto>> GetAll()
        {
            return await this.mediator.Send(new GetAllQuery<IEnumerable<WindowDto>>());
        }

        /// <summary>
        /// Получить окно за Id
        /// </summary>
        /// <param name="id">Id окна</param>
        /// <returns>Окно</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<WindowDto> Get(int id)
        {
            return await this.mediator.Send(new GetByIdQuery<WindowDto>(id));
        }

        /// <summary>
        /// Добавить новое или сохранить существующее окно
        /// </summary>
        /// <param name="window">Окно</param>
        /// <returns>Id окна</returns>
        [HttpPost]
        public async Task<int> Post([FromBody] WindowDto window)
        {
            return await this.mediator.Send(new SaveCommand<WindowDto>(window));
        }

        /// <summary>
        /// Удалить окна за Id
        /// </summary>
        /// <param name="id">Id окна</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(int id)
        {
            await this.mediator.Send(new DeleteCommand<WindowDto>(id));
        }
    }
}
