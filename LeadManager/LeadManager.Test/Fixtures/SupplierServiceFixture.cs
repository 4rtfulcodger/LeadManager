using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
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
    public class SupplierServiceFixture: IDisposable
    {
        public ILogger<SupplierService> Logger { get; }
        public ISupplierRepository TestSupplierRepository { get; }
      
        public SupplierService SupplierService { get; }
        public SupplierServiceFixture()
        {
            Logger = new Mock<ILogger<SupplierService>>().Object;
            var TestSupplierRepositoryMock = new Mock<ISupplierRepository>();

            IQueryable<Supplier> queryableSupplierList = new List<Supplier>().AsQueryable();
            var pagaedList = PagedList<Supplier>.Create(queryableSupplierList, 1, 10);

            TestSupplierRepositoryMock.Setup(m => m.GetSuppliersAsync(It.IsAny<SupplierFilter>())).Returns(pagaedList);
            TestSupplierRepositoryMock.Setup(m => m.AddSupplierAsync(It.IsAny<Supplier>())).ReturnsAsync(true);
            TestSupplierRepositoryMock.Setup(m => m.UpdateSupplierAsync(It.IsAny<int>())).ReturnsAsync(true);
            TestSupplierRepositoryMock.Setup(m => m.DeleteSupplier(It.IsAny<Supplier>())).ReturnsAsync(true);

            TestSupplierRepository = TestSupplierRepositoryMock.Object;
            SupplierService = new SupplierService(TestSupplierRepository, Logger);
        }

        public void Dispose()
        {
            //Add clean up code here if required
        }
    }
}
