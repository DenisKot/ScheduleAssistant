using System.Collections.Generic;
using System.Threading.Tasks;
using ScheduleAssistant.IntegrationTests.Utilities;
using NUnit.Framework;
using ScheduleAssistant.Communication.Windows;

namespace ScheduleAssistant.IntegrationTests.Controllers
{
    public class WindowsControllerIntegrationTests : InMemoryBaseIntegrationTest
    {
        [Test]
        public async Task GetAllWindows_ReturnsNotEmptyResult()
        {
            // Arrange
            var url = $"/api/v1/window/all";

            // Act
            var response = await this.client.GetAsync(url);
            var result = await response.DeserializeContentAsync<IEnumerable<WindowDto>>();

            // Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }
    }
}
