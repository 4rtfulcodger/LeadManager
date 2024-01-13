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
using LeadManager.Test.Fixtures;

namespace LeadManager.Test
{
    public class SourceServiceTests : IClassFixture<SourceServiceFixture>
    {
        private readonly SourceServiceFixture _fixture;

        public SourceServiceTests(SourceServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SourceService_CreateSourceAsync_ReturnsTrueWhenCalled()
        {            

            //Arrange
            var newSource = new Source( "TestName","TestDescription");
            var result = await _fixture.SourceService.CreateSourceAsync(newSource);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SourceService_CreateSourceAsync_AddsANewSourceToRepository()
        {
            //Arrange
            var sourceCount = _fixture.TestSourceReporitory.GetSourcesAsync().Result.Count();            
            var newSource = new Source("TestName1", "TestDescription1");

            //Act
            var result = await _fixture.SourceService.CreateSourceAsync(newSource);

            //Assert
            Assert.Equal(_fixture.TestSourceReporitory.GetSourcesAsync().Result.Count(), sourceCount + 1);
        }

        [Fact]
        public async Task SourceService_GetSourceWithIdAsync_ReturnsSource()
        {
            //Arrange
            for (int i = 2; i<=5; i++)
              await _fixture.SourceService.CreateSourceAsync(new Source($"TestName{i}", $"TestDescription{i}"));
            
             
            //Act
            var source = await _fixture.TestSourceReporitory.GetSourceWithIdAsync(5);

            //Assert
            Assert.NotNull(source);
            Assert.Equal("TestName5", source.Name);
            Assert.Equal("TestDescription5", source.Description);
        }
        
        [Fact]
        public async Task SourceService_GetSourcesAsyncWithFilter_ReturnsPagedListOfSource()
        {
            //Act
            var sources = await _fixture.SourceService.GetSourcesAsync(new SourceFilter{ PageSize=5, PageNumber=1 });
            
            //Assert
            Assert.IsType<PagedList<Source>>(sources);
        }

        [Fact]
        public async Task SourceService_UpdateSourceAsync_ReturnsBoolean()
        {     
            //Act
            var updateResult = await _fixture.TestSourceReporitory.UpdateSourceAsync(1);

            //Assert
            Assert.IsType<bool>(updateResult);
        }

        [Fact]
        public async Task SourceService_DeleteSource_ReturnsBoolean()
        {
            //Act
            var source = await _fixture.TestSourceReporitory.GetSourceWithIdAsync(1);
            var deleteResult = await _fixture.SourceService.DeleteSource(source);

            //Assert
            Assert.IsType<bool>(deleteResult);
        }
    }
}
