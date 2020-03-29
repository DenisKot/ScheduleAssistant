using System.Threading.Tasks;
using ScheduleAssistant.Application.CommandsAndQueries.Generic;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Domain;
using NUnit.Framework;

namespace ScheduleAssistant.IntegrationTests.CommandsAndQueries
{
    [TestFixture]
    public abstract class BaseCommandTest<TDomain, TDto> : BaseRequestTest<TDomain>
        where TDomain : BaseEntity
        where TDto : BaseEntityDto
    {
        private int entityId;
        private TDto entityDto;

        [Test, Order(1)]
        public async Task SaveCommand_InsertsEntity_ReturnsIdAndInsertToDb()
        {
            var beforeCount = await this.Repository.CountAsync();
            this.entityDto = this.BuildDto();

            this.entityId = await this.Mediator.Send(new SaveCommand<TDto>(entityDto));
            this.entityDto.Id = this.entityId;

            var afterCount = await this.Repository.CountAsync();

            Assert.NotZero(this.entityId);
            Assert.AreEqual(beforeCount + 1, afterCount);
        }

        [Test, Order(2)]
        public async Task SaveCommand_UpdatesEntity_FieldsAreUpdated()
        {
            if(this.entityId == 0)
                Assert.Ignore();

            this.UpdateDto(this.entityDto);

            await this.Mediator.Send(new SaveCommand<TDto>(this.entityDto));
            var retrievedDto = await this.Mediator.Send(new GetByIdQuery<TDto>(this.entityId));
            var dtosAreEqual = this.CheckIfDtoChanged(retrievedDto);

            Assert.IsTrue(dtosAreEqual);
        }

        [Test, Order(3)]
        public async Task SaveCommand_UpdatesEntity_ReturnsIdAndInsertToDb()
        {
            if(this.entityId == 0)
                Assert.Ignore();

            await this.Mediator.Send(new DeleteCommand<TDto>(this.entityId));
            var dbEntity = await this.Repository.FirstOrDefaultAsync(e => e.Id == this.entityId);
            
            Assert.IsNull(dbEntity);
        }

        protected abstract TDto BuildDto();
        protected abstract void UpdateDto(TDto dto);
        protected abstract bool CheckIfDtoChanged(TDto dto);
    }
}