using ScheduleAssistant.IntegrationTests.Utilities;

namespace ScheduleAssistant.IntegrationTests
{
    public class InMemoryBaseIntegrationTest : BaseIntegrationTest
    {
        public InMemoryBaseIntegrationTest() : base(new InMemoryWebApplicationFactory())
        {
        }
    }
}