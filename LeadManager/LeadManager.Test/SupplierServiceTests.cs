using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Test.Fixtures;

namespace LeadManager.Test
{
    public class SupplierServiceTests: IClassFixture<SupplierServiceFixture>
    {
        private readonly SupplierServiceFixture _fixture;

        public SupplierServiceTests(SupplierServiceFixture fixture)
        {
            _fixture = fixture;
        }        

        [Fact]
        public async Task SupplierService_AddSupplierAsync_ReturnsTrueWhenCalled()
        {

            //Arrange
            var newSupplier = new Supplier("TestSourceName", "TestSourceDescription");
            var result = await _fixture.SupplierService.AddSupplierAsync(newSupplier);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SupplierService_GetSupplierWithIdAsync_ReturnsSupplier()
        {
            //Arrange
            for (int i = 1; i <= 5; i++)
                await _fixture.SupplierService.AddSupplierAsync(new Supplier($"TestSourceName{i}", $"TestSourceDescription{i}"));

            //Act
            var supplier = await _fixture.TestSupplierReporitory.GetSupplierWithIdAsync(5);

            //Assert
            Assert.NotNull(supplier);
        }

        [Fact]
        public async Task SupplierService_GetSuppliersAsyncWithFilter_ReturnsPagedListOfSupplier()
        {
            //Act
            var suppliers = await _fixture.SupplierService.GetSuppliersAsync(new SupplierFilter { PageSize = 5, PageNumber = 1 });

            //Assert
            Assert.IsType<PagedList<Supplier>>(suppliers);
        }

        [Fact]
        public async Task SupplierService_UpdateSupplierAsync_ReturnsBoolean()
        {
            //Act
            var updateResult = await _fixture.TestSupplierReporitory.UpdateSupplierAsync(1);

            //Assert
            Assert.IsType<bool>(updateResult);
        }

        [Fact]
        public async Task SupplierService_DeleteSupplier_ReturnsBoolean()
        {
            //Act
            var supplier = await _fixture.TestSupplierReporitory.GetSupplierWithIdAsync(1);
            var deleteResult = await _fixture.SupplierService.DeleteSupplier(supplier);

            //Assert
            Assert.IsType<bool>(deleteResult);
        } 
    }
}
