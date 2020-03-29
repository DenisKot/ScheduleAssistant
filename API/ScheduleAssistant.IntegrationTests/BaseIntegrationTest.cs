using System.Net.Http;
using ScheduleAssistant.IntegrationTests.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace ScheduleAssistant.IntegrationTests
{
    public abstract class BaseIntegrationTest
    {
        public const string MEDIA_TYPE = "application/json";

        protected HttpClient client;

        private readonly APIWebApplicationFactory factory;

        public BaseIntegrationTest() : this(new APIWebApplicationFactory()) { }

        public BaseIntegrationTest(APIWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [OneTimeSetUp]
        public void SetUpClient()
        {
            this.client = this.GetClient();
        }

        protected HttpClient GetClient(bool allowAutoRedirect = false)
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            };
            var client = this.factory.CreateClient(options);

            return client;
        }
    }
}
