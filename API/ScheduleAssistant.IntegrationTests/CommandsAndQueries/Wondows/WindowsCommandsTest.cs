using ScheduleAssistant.Domain.Windows;
using ScheduleAssistant.Communication.Windows;
using System;

namespace ScheduleAssistant.IntegrationTests.CommandsAndQueries.Questionnaires
{
    public class WindowsCommandsTest : BaseCommandTest<ExpressWindow, WindowDto>
    {
        protected override WindowDto BuildDto()
        {
            return new WindowDto
            {
                Name = "Сегодня как можно раньше",
                Description = "Доставка за несколько часов",
                Price = 99,
                Type = WindowTypeDto.ExpressDelivery,
                Available = true
            };
        }

        protected override void UpdateDto(WindowDto dto)
        {
            dto.Name = "New title";
        }

        protected override bool CheckIfDtoChanged(WindowDto dto)
        {
            return dto.Name == "New title";
        }
    }
}