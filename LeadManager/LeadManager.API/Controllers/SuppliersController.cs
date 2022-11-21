using LeadManager.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<IEnumerable<SourceDto>> GetSuppliers()
        {
            return Ok(TestDataStore.Current.Suppliers);
        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public ActionResult<SupplierDto> GetSupplier(int id)
        {
            var supplierToReturn = TestDataStore.Current.Suppliers.FirstOrDefault(x => x.Id == id);

            if (supplierToReturn == null)
                return NotFound();

            return Ok(supplierToReturn);
        }

        [HttpPost]
        public ActionResult<SourceDto> CreateSupplier(SupplierForCreateDto supplier)
        {
            var newSupplier = new SupplierDto
            {
                Id = TestDataStore.Current.Suppliers.Count() + 1,
                Name = supplier.Name,
                Description = supplier.Description
            };

            TestDataStore.Current.Suppliers.Add(newSupplier);

            return CreatedAtRoute("GetSupplier", new { id = newSupplier.Id }, newSupplier);

        }
    }
}
