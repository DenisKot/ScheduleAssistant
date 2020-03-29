using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScheduleAssistant.Application.CommandsAndQueries.Generic;
using ScheduleAssistant.Communication;
using ScheduleAssistant.Domain;
using NUnit.Framework;

namespace ScheduleAssistant.IntegrationTests.CommandsAndQueries
{
    [TestFixture]
    public class BaseQueryTest<TDomain, TDto> : BaseRequestTest<TDomain>
        where TDomain: BaseEntity
        where TDto: BaseEntityDto
    {
        [Test]
        public async Task GetAllQuery_ReturnsDbEntitiesCount()
        {
            var entitiesCount = await this.Repository.CountAsync();

            var entities = await this.Mediator.Send(new GetAllQuery<IEnumerable<TDto>>());

            Assert.AreEqual(entitiesCount, entities.Count());
        }

        [Test]
        public async Task GetByIdQuery_ReturnsFirstEntityFromDb()
        {
            var entity = await this.Repository.FirstOrDefaultAsync(e => true);

            if(entity == null)
                Assert.Ignore($"Table doesn't contains any record of type {nameof(TDomain)}.  Omitting.");

            var resultDto = await this.Mediator.Send(new GetByIdQuery<TDto>(entity.Id));

            Assert.IsNotNull(resultDto);
            Assert.AreEqual(entity.Id, resultDto.Id);
        }
    }
}