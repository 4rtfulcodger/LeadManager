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

namespace LeadManager.Test
{
    public class LeadsControllerTests
    {
        [Fact]
        public async Task LeadsController_GetLead_ReturnsIActionResult()
        {
            var logger = new Mock<ILogger<FilesController>>().Object;
            var testSupplierRepositoryMock = new Mock<ISupplierRepository>();

            var emailServiceMock = new Mock<IEmailService>();
            var sourceServiceMock = new Mock<ISourceService>();
            var supplierServiceMock = new Mock<ISupplierService>();
            var leadServiceMock = new Mock<ILeadService>();
            //var mapperMock = new Mock<IMapper>();
            //mapperMock.Setup(m => m.Map<LeadDto>(It.IsAny<Lead>())).Returns(new LeadDto() { Id = 1100 });

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<LeadInfoProfile>());
            var mapper = new Mapper(mapperConfiguration);

            //var apiResponseHandler = new ApiResponseHandler(mapperMock.Object);
            var apiResponseHandler = new ApiResponseHandler(mapper);

            leadServiceMock.Setup(m => m.GetLeadWithIdAsync(1,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>())).
                ReturnsAsync(new Core.Entities.Lead(1, 1, "TESTNAME", "TESTDES"));

            var leadsController = new LeadsController(logger,
                emailServiceMock.Object,
                sourceServiceMock.Object,
                leadServiceMock.Object,
                supplierServiceMock.Object,
                mapper,
                apiResponseHandler);

            var iActionResult = await leadsController.GetLead(1);
            var objectResult = iActionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.IsType<LeadDto>(objectResult.Value);
        }
    }
}
