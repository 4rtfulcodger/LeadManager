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
        public ISupplierRepository TestSupplierReporitory { get; }
        public SupplierService SupplierService { get; }
        public SupplierServiceFixture()
        {
            Logger = new Mock<ILogger<SupplierService>>().Object;
            TestSupplierReporitory = new TestSupplierRepository();
            SupplierService = new SupplierService(TestSupplierReporitory, Logger);
        }

        public void Dispose()
        {
            //Add clean up code here if required
        }
    }
}
