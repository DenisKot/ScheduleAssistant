using System;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain;
using ScheduleAssistant.IntegrationTests.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace ScheduleAssistant.IntegrationTests.CommandsAndQueries
{
    public class BaseRequestTest<TDomain>
        where TDomain : BaseEntity
    {
        protected IMediator Mediator;
        protected IRepository<TDomain> Repository => this.Resolve<IRepository<TDomain>>();

        private IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void SetUpMediator()
        {
            this.Mediator = this.CreateMediator();
        }

        protected T Resolve<T>()
        {
            return (T) this.serviceProvider.GetService(typeof(T));
        }

        private IMediator CreateMediator()
        {
            //  Note: To run db in memory use InMemoryWebApplicationFactory
            var factory = new APIWebApplicationFactory();
            var options = new WebApplicationFactoryClientOptions();

            factory.CreateClient(options);
            this.serviceProvider = factory.Services;
            var mediator = (IMediator) factory.Services.GetService(typeof(IMediator));

            return mediator;
        }
    }
}