using System;
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
    public class SaveWindowCommandHandlerTest
    {
        private SaveWindowCommandHandler handler;
        private Mock<IRepository<ExpressWindow>> repositoryMock;
        private Mock<IMappingService> mappingServiceMock;

        public SaveWindowCommandHandlerTest()
        {
            this.repositoryMock = new Mock<IRepository<ExpressWindow>>();
            this.mappingServiceMock = new Mock<IMappingService>();

            this.handler = new SaveWindowCommandHandler(this.repositoryMock.Object, this.mappingServiceMock.Object);
        }

        [Test]
        public async Task Handle_WithNullEntity_ThrowsException()
        {
            var request = new SaveWindowCommand {EntityDto = null};

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await this.handler.Handle(request, CancellationToken.None)
            );
        }

        [Test, AutoData]
        public async Task Handle_WithDiffrentDates_ThrowsException(WindowDto dto)
        {
            var request = new SaveWindowCommand { EntityDto = dto };
            dto.Start = DateTimeOffset.Now;
            dto.Finish = DateTimeOffset.Now.AddDays(2);

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await this.handler.Handle(request, CancellationToken.None)
            );

            Assert.That(ex.Message, Is.EqualTo("Start and finish dates should be the same"));
        }

        [Test, AutoData]
        public async Task Handle_WithDtoIdZero_InsertsEntity(WindowDto dto, ExpressWindow entity)
        {
            dto.Id = 0;
            dto.Finish = dto.Start.AddHours(2);
            this.mappingServiceMock.Setup(x => x.Map<ExpressWindow>(It.IsAny<WindowDto>())).Returns(entity);
            var request = new SaveWindowCommand { EntityDto = dto };

            var result = await this.handler.Handle(request, CancellationToken.None);

            this.repositoryMock.Verify(x => x.InsertAsync(entity));
            this.repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Test, AutoData]
        public async Task Handle_WithDtoIdZero_ReturnEntityId(WindowDto dto, ExpressWindow entity)
        {
            dto.Id = 0;
            dto.Finish = dto.Start.AddHours(2);
            this.mappingServiceMock.Setup(x => x.Map<ExpressWindow>(It.IsAny<WindowDto>())).Returns(entity);
            var request = new SaveWindowCommand { EntityDto = dto };

            var result = await this.handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(entity.Id, result);
        }

        [Test, AutoData]
        public async Task Handle_WithDtoIdNotZero_UpdatesEntity(WindowDto dto, ExpressWindow entity)
        {
            dto.Finish = dto.Start.AddHours(2);
            this.repositoryMock.Setup(x =>
                    x.FirstOrDefaultAsync(dto.Id))
                .ReturnsAsync(entity);
            var request = new SaveWindowCommand { EntityDto = dto };

            var result = await this.handler.Handle(request, CancellationToken.None);

            this.mappingServiceMock.Verify(x => x.Map<WindowDto, ExpressWindow>(dto, entity));
            this.repositoryMock.Verify(x => x.UpdateAsync(entity));
            this.repositoryMock.Verify(x => x.SaveChangesAsync());
        }
    }
}
