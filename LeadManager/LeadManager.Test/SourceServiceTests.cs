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
            //Arrange
            var newSource = new Source( "TestName","TestDescription");
            var result = await _SourceService.CreateSourceAsync(newSource);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SourceService_CreateSourceAsync_AddsANewSourceToRepository()
        {
            //Arrange
            var sourceCount =  _TestSourceReporitory.GetSourcesAsync().Result.Count();            
            var newSource = new Source("TestName", "TestDescription");

            //Act
            var result = await _SourceService.CreateSourceAsync(newSource);

            //Assert
            Assert.Equal(_TestSourceReporitory.GetSourcesAsync().Result.Count(), sourceCount + 1);
        }

        [Fact]
        public async Task SourceService_GetSourceWithIdAsync_ReturnsSource()
        {
            //Arrange
            for (int i = 0; i<=3; i++)
            {
                await _SourceService.CreateSourceAsync(new Source($"TestName{i}", $"TestDescription{i}"));
            }
             
            //Act
            var sourceCount = await _TestSourceReporitory.GetSourceWithIdAsync(3);

            //Assert
            Assert.NotNull(sourceCount);
        }
        
        [Fact]
        public async Task SourceService_GetSourcesAsyncWithFilter_ReturnsPagedListOfSource()
        {
            //Act
            var sources = await _SourceService.GetSourcesAsync(new SourceFilter{ PageSize=5, PageNumber=1 });
            
            //Assert
            Assert.IsType<PagedList<Source>>(sources);
        }

        [Fact]
        public async Task SourceService_UpdateSourceAsync_ReturnsBoolean()
        {     
            //Act
            var updateResult = await _TestSourceReporitory.UpdateSourceAsync(1);

            //Assert
            Assert.IsType<bool>(updateResult);
        }

        [Fact]
        public async Task SourceService_DeleteSource_ReturnsBoolean()
        {
            //Act
            var source = await _TestSourceReporitory.GetSourceWithIdAsync(1);
            var deleteResult = await _TestSourceReporitory.DeleteSource(source);

            //Assert
            Assert.IsType<bool>(deleteResult);
        }
    }
}
