using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Test
{    

    public class LeadServiceTests : IClassFixture<LeadServiceFixture>
    {
        private readonly LeadServiceFixture _fixture;

        public LeadServiceTests(LeadServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task LeadService_CreateLeadTypeAsync_ReturnsTrueWhenCalled()
        {

            //Arrange
            var newSLeadType = new LeadType() { Name="testleadtype", Description="testleaddescription" };
            var result = await _fixture.LeadService.CreateLeadTypeAsync(newSLeadType);

            //Assert
            Assert.True(result);
        }
    }
}
