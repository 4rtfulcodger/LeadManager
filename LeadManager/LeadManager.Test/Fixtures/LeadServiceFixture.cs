using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Infrastructure.Services;
using LeadManager.Test.TestRepositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Test.Fixtures
{
    
    public class LeadServiceFixture : IDisposable
    {
        public ILogger<LeadService> Logger { get; }
        public ILeadRepository TestLeadRepository { get; }

        public ILeadTypeRepository TestLeadTypeRepository { get; }

        public ILeadAttributeRepository TestLeadAttributeRepository  { get; }

        public LeadService LeadService { get; }
        public LeadServiceFixture()
        {
            Logger = new Mock<ILogger<LeadService>>().Object;
            var TestLeadRepositoryMock = new Mock<ILeadRepository>();
            var TestLeadTypeRepositoryMock = new Mock<ILeadTypeRepository>();
            var TestLeadAttributeRepositoryMock = new Mock<ILeadAttributeRepository>();

            IQueryable<Lead> queryableLeadList = new List<Lead>().AsQueryable();
            var pagedLeadList = PagedList<Lead>.Create(queryableLeadList, 1, 10);

            IQueryable<LeadType> queryableLeadTypeList = new List<LeadType>().AsQueryable();
            var pagedLeadTypeList = PagedList<LeadType>.Create(queryableLeadTypeList, 1, 10);

            TestLeadTypeRepositoryMock.Setup(m => m.CreateLeadTypeAsync(It.IsAny<LeadType>())).ReturnsAsync(true);
            TestLeadTypeRepositoryMock.Setup(m => m.UpdateLeadTypeAsync(It.IsAny<int>())).ReturnsAsync(true);
            TestLeadTypeRepositoryMock.Setup(m => m.DeleteLeadType(It.IsAny<LeadType>())).ReturnsAsync(true);
            TestLeadTypeRepositoryMock.Setup(m => m.GetLeadTypeAsync(It.IsAny<int>())).ReturnsAsync(new LeadType());
            TestLeadTypeRepositoryMock.Setup(m => m.GetLeadTypesAsync(It.IsAny<LeadTypeFilter>())).Returns(pagedLeadTypeList);

            TestLeadRepositoryMock.Setup(m => m.CreateLeadAsync(It.IsAny<Lead>())).ReturnsAsync(true);
            TestLeadRepositoryMock.Setup(m => m.UpdateLeadAsync(It.IsAny<int>())).ReturnsAsync(true);
            TestLeadRepositoryMock.Setup(m => m.DeleteLead(It.IsAny<Lead>())).ReturnsAsync(true);
            TestLeadRepositoryMock.Setup(m => m.GetLeadsAsync(It.IsAny<LeadFilter>())).Returns(pagedLeadList);
            TestLeadRepositoryMock.Setup(m => m.GetLeadWithIdAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new Lead(1,1,"testlname","testldescription"));

            TestLeadAttributeRepositoryMock.Setup(m => m.CreateLeadAttributeAsync(It.IsAny<LeadAttribute>())).ReturnsAsync(true);
            TestLeadAttributeRepositoryMock.Setup(m => m.UpdateLeadAttributeAsync(It.IsAny<int>())).ReturnsAsync(true);
            TestLeadAttributeRepositoryMock.Setup(m => m.DeleteLeadAttribute(It.IsAny<LeadAttribute>())).ReturnsAsync(true);
            TestLeadAttributeRepositoryMock.Setup(m => m.GetLeadAttributeAsync(It.IsAny<int>())).ReturnsAsync(new LeadAttribute());
            TestLeadAttributeRepositoryMock.Setup(m => m.GetLeadAttributesAsync(It.IsAny<int>())).ReturnsAsync(new List<LeadAttribute>());

            TestLeadRepository = TestLeadRepositoryMock.Object;
            TestLeadTypeRepository = TestLeadTypeRepositoryMock.Object;
            TestLeadAttributeRepository = TestLeadAttributeRepositoryMock.Object;
            LeadService = new LeadService(TestLeadRepository, TestLeadTypeRepository,TestLeadAttributeRepository, Logger);
        }

        public void Dispose()
        {
            //Add clean up code here if required
        }
    }
}
