using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ScheduleAssistant.Application.Windows;
using ScheduleAssistant.Common;
using ScheduleAssistant.Common.Exceptions;
using ScheduleAssistant.Communication.Windows;
using ScheduleAssistant.Data.Repository;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.UnitTests
{
    public class HorizonQueryHandlerTest
    {
        private HorizonQueryHandler handler;
        private Mock<IRepository<Window>> repositoryMock;
        private Mock<IMappingService> mappingServiceMock;

        public HorizonQueryHandlerTest()
        {
            this.repositoryMock = new Mock<IRepository<Window>>();
            this.mappingServiceMock = new Mock<IMappingService>();

            this.handler = new HorizonQueryHandler(this.repositoryMock.Object, this.mappingServiceMock.Object);
        }

        [Test]
        public async Task Handle_NegativeHorizon_ThrowsException()
        {
            var horizon = -1;
            var request = new HorizonQuery { Horizon = horizon };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => 
                await this.handler.Handle(request, CancellationToken.None)
            );

            Assert.That(ex.Message, Is.EqualTo("Horizon days can't be negative"));
        }

        [Test]
        public async Task Handle_CurrentDateMin_ThrowsException()
        {
            var horizon = 5;
            var currentDate = DateTimeOffset.MinValue;
            var request = new HorizonQuery { Horizon = horizon, CurrentDate = currentDate };

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await this.handler.Handle(request, CancellationToken.None)
            );

            Assert.That(ex.Message, Is.EqualTo("CurrentDate must be defined"));
        }


        [Test, AutoData]
        public async Task Handle_WithoutUsualWindows_ReturnsOnlyOneExpressWindow(Window window)
        {
            var expectedCount = 1;
            this.repositoryMock
                .Setup(x => x.GetAllListAsync(It.IsAny<Expression<Func<Window, bool>>>()))
                .ReturnsAsync(new List<Window>());
            this.repositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Window, bool>>>()))
                .ReturnsAsync(window);
            var request = new HorizonQuery { Horizon = 1, CurrentDate = DateTimeOffset.Now };

            await this.handler.Handle(request, CancellationToken.None);

            this.mappingServiceMock.Verify(x => 
                x.Map<IEnumerable<WindowDto>>(It.Is<List<Window>>(m => m.Count() == expectedCount)));
        }

        [Test, AutoData]
        public async Task Handle_WithUsualAndExpressWindows_ReturnsUnion(Window express, Window usual1, Window usual2)
        {
            var usualList = new List<Window> {usual1, usual2};
            var expressCount = 1;
            var expectedListCount = usualList.Count + expressCount;
            this.repositoryMock
                .Setup(x => x.GetAllListAsync(It.IsAny<Expression<Func<Window, bool>>>()))
                .ReturnsAsync(new List<Window> { usual1, usual2 });
            this.repositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Window, bool>>>()))
                .ReturnsAsync(express);
            var request = new HorizonQuery { Horizon = 1, CurrentDate = DateTimeOffset.Now };

            await this.handler.Handle(request, CancellationToken.None);

            this.mappingServiceMock.Verify(x =>
                x.Map<IEnumerable<WindowDto>>(It.Is<List<Window>>(m => m.Count == expectedListCount)));
        }
    }
}
