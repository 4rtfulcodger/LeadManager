using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task LeadService_CreateLeadTypeAsync_ReturnsBooleanWhenCalled()
        {

            //Arrange
            var newLeadType = new LeadType() { Name="testleadtype", Description="testleaddescription" };
            var result = await _fixture.LeadService.CreateLeadTypeAsync(newLeadType);

            //Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task LeadService_CreateLeadAsync_ReturnsBooleanWhenCalled()
        {

            //Arrange
            var newLead = new Lead(1,1,"test lead name", "test lead description");
            var result = await _fixture.LeadService.CreateLeadAsync(newLead);

            //Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task LeadService_CreateLeadAttributeAsync_ReturnsBooleanWhenCalled()
        {

            //Arrange            
            var result = await _fixture.LeadService.CreateLeadAttributeAsync(new LeadAttribute());

            //Assert
            Assert.IsType<bool>(result);
        }
        

        [Fact]
        public async Task LeadService_DeleteLeadType_ReturnsBooleanWhenCalled()
        {

            //Arrange
            var result = await _fixture.LeadService.DeleteLeadType(new LeadType());

            //Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task LeadService_DeleteLeadAttribute_ReturnsBooleanWhenCalled()
        {

            //Arrange
            var result = await _fixture.LeadService.DeleteLeadAttribute(new LeadAttribute());

            //Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task LeadService_DeleteLead_ReturnsBooleanWhenCalled()
        {
            //Arrange
            var lead = new Lead(1, 1, "test lead name", "test lead description");
            var result = await _fixture.LeadService.DeleteLead(lead);

            //Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task LeadService_GetLeadsAsync_ReturnsPagedListOfLead()
        {
            //Act
            var leads = await _fixture.LeadService.GetLeadsAsync(new LeadFilter { PageSize = 5, PageNumber = 1 });

            //Assert
            Assert.IsType<PagedList<Lead>>(leads);
        }
    }
}
