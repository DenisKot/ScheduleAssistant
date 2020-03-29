using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScheduleAssistant.IntegrationTests.Utilities;
using NUnit.Framework;
using ScheduleAssistant.Communication.Windows;

namespace ScheduleAssistant.IntegrationTests.Controllers
{
    public class HorizonControllerIntegrationTests : InMemoryBaseIntegrationTest
    {
        [Test]
        public async Task GetHorizon_ReturnsNotEmptyList()
        {
            // Arrange
            var horizon = 5;
            var dateTime = DateTimeOffset.Now;
            var url = $"/api/v1/horizon?horizon={horizon}&currentDate={dateTime.ToString("s")}";

            // Act
            var response = await this.client.GetAsync(url);
            var result = await response.DeserializeContentAsync<IEnumerable<WindowDto>>();

            // Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }
    }
}
