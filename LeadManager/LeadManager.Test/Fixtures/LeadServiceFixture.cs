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

            //IQueryable<Supplier> queryableSupplierList = new List<Supplier>().AsQueryable();
            //var pagedList = PagedList<Supplier>.Create(queryableSupplierList, 1, 10);

            TestLeadTypeRepositoryMock.Setup(m => m.CreateLeadTypeAsync(It.IsAny<LeadType>())).ReturnsAsync(true);

            //TestLeadRepositoryMock.Setup(m => m.GetSuppliersAsync(It.IsAny<SupplierFilter>())).Returns(pagaedList);
            //TestLeadRepositoryMock.Setup(m => m.AddSupplierAsync(It.IsAny<Supplier>())).ReturnsAsync(true);
            //TestLeadRepositoryMock.Setup(m => m.UpdateSupplierAsync(It.IsAny<int>())).ReturnsAsync(true);
            //TestLeadRepositoryMock.Setup(m => m.DeleteSupplier(It.IsAny<Supplier>())).ReturnsAsync(true);

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
