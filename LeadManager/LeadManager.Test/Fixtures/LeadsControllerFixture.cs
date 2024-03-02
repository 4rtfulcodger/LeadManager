using LeadManager.API.Controllers;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LeadManager.API.BusinessLogic.Common;
using Serilog.Core;
using LeadManager.API.Profiles;
using LeadManager.Core.Entities;

namespace LeadManager.Test.Fixtures
{
    public class LeadsControllerFixture: IDisposable
    {        
        Mock<ILeadService> _leadServiceMock;
        Mock<IEmailService> _emailServiceMock;
        Mock<ISourceService> _sourceServiceMock;
        Mock<ISupplierService> _supplierServiceMock;

        public LeadsController LeadsController { get; }      
        public LeadsControllerFixture()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<LeadInfoProfile>());
            var mapper = new Mapper(mapperConfiguration);
            var apiResponseHandler = new ApiResponseHandler(mapper);

            IQueryable<Lead> queryableLeadList = new List<Lead>() { new Lead(1, 1, "TESTNAME", "TESTDES") }.AsQueryable();
            var pagedLeadList = PagedList<Lead>.Create(queryableLeadList, 1, 10);

            _leadServiceMock = new Mock<ILeadService>();
            
            _leadServiceMock.Setup(m => m.GetLeadWithIdAsync(1,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>())).
                ReturnsAsync(new Lead(1, 1, "TESTNAME", "TESTDES"));

            _leadServiceMock.Setup(m => m.GetLeadsAsync(It.IsAny<LeadFilter>())).
                Returns(pagedLeadList);

            _emailServiceMock = new Mock<IEmailService>();
            _sourceServiceMock = new Mock<ISourceService>();
            _supplierServiceMock = new Mock<ISupplierService>();

            LeadsController = new LeadsController(new Mock<ILogger<LeadsController>>().Object,
                _emailServiceMock.Object,
                _sourceServiceMock.Object,
                _leadServiceMock.Object,
                _supplierServiceMock.Object,
                mapper,
                apiResponseHandler);
        }

        public void Dispose()
        {
            //Add clean up code here if required
        }
    }
}
