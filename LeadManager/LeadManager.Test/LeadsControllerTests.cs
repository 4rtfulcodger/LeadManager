using AutoMapper;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadManager.API.Controllers;
using LeadManager.API.BusinessLogic.Common;
using LeadManager.Core.ViewModels;
using LeadManager.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LeadManager.API.Profiles;
using LeadManager.Test.Fixtures;

namespace LeadManager.Test
{
    public class LeadsControllerTests: IClassFixture<LeadsControllerFixture>
    {

        private readonly LeadsControllerFixture _fixture;

        public LeadsControllerTests()
        {
            _fixture = new LeadsControllerFixture();
        }

        [Fact]
        public async Task LeadsController_GetLead_ReturnsIActionResult()
        {
            var iActionResult = await _fixture.LeadsController.GetLead(1);
            var objectResult = iActionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.IsType<LeadDto>(objectResult.Value);
        }

        [Fact]
        public async Task LeadsController_GetLeads_ReturnsIActionResult()
        {
            var iActionResult = await _fixture.LeadsController.GetLeads(new Core.Helpers.LeadFilter{
                FromCreatedDate=DateTime.Now.AddDays(-7), 
                ToCreatedDate= DateTime.Now
                });
            var objectResult = iActionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<LeadDto>>(objectResult.Value);
        }
    }
}
