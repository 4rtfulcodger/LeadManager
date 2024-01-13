using LeadManager.Core.Interfaces.Source;
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
    public class SourceServiceFixture: IDisposable
    {
        public ILogger<SourceService> Logger { get; }
        public ISourceRepository TestSourceReporitory { get; }
        public SourceService SourceService { get; }
        public SourceServiceFixture()
        {
            Logger = new Mock<ILogger<SourceService>>().Object;
            TestSourceReporitory = new TestSourceRepository();
            SourceService = new SourceService(TestSourceReporitory, Logger);
        }

        public void Dispose()
        {
            //Add clean up code here if required
        }
    }
}
