using LeadManager.Core.Entities.Source;
using LeadManager.Core.Helpers;
using LeadManager.Infrastructure.Services;
using LeadManager.Test.TestRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace LeadManager.Test
{
    public class SourceServiceTests
    {
        private readonly ILogger<SourceService> _logger;
        private TestSourceRepository _TestSourceReporitory;
        private SourceService _SourceService;

        public SourceServiceTests()
        {
            _logger = new Mock<ILogger<SourceService>>().Object;
            _TestSourceReporitory = new TestSourceRepository();
            _SourceService = new SourceService(_TestSourceReporitory, _logger);
        }


        [Fact]
        public async Task SourceService_CreateSourceAsync_ReturnsTrueWhenCalled()
        {
            
            var newSource = new Source( "TestName","TestDescription");
            var result = await _SourceService.CreateSourceAsync(newSource);

            Assert.True(result);
        }

        [Fact]
        public async Task SourceService_CreateSourceAsync_AddsANewSourceToRepository()
        {
            var sourceCount =  _TestSourceReporitory.GetSourcesAsync().Result.Count();
            
            var newSource = new Source("TestName", "TestDescription");
            var result = await _SourceService.CreateSourceAsync(newSource);

            Assert.Equal(_TestSourceReporitory.GetSourcesAsync().Result.Count(), sourceCount + 1);
        }
    }
}
