using AutoMapper;
using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Interfaces;
using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService,
            ILogger<FilesController> logger,
            IEmailService emailService,
            IMapper mapper)
        {
            _supplierService = supplierService ?? throw new ArgumentException(nameof(supplierService)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _mapper = mapper;
        }


        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            return Ok(_mapper.Map<SupplierDto[]>(await _supplierService.GetSuppliersAsync()));
        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierWithIdAsync(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier(SupplierForCreateDto supplierDto)
        {
            var newSupplier = _mapper.Map<Supplier>(supplierDto);
            bool addLeadSuccess = await _supplierService.AddSupplierAsync(newSupplier);

            return CreatedAtRoute("GetSource", new { id = newSupplier.SupplierId }, _mapper.Map<SupplierDto>(newSupplier));

        }

        [HttpPatch("{supplierId}")]
        public async Task<ActionResult<LeadDto>> UpdateSupplier(JsonPatchDocument<SupplierForUpdateDto> patchDocument, int supplierId)
        {
            var supplierEntity = await _supplierService.GetSupplierWithIdAsync(supplierId);
            if (supplierEntity == null)
                return NotFound();

            var supplierDto = _mapper.Map<SupplierForUpdateDto>(supplierEntity);
            patchDocument.ApplyTo(supplierDto);

            _mapper.Map(supplierDto, supplierEntity);
            bool updateresult = await _supplierService.UpdateSupplierAsync(supplierId);

            if (updateresult)
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }

        }

        [HttpDelete("{id}", Name = "DeleteSupplier")]
        public async Task<ActionResult<SupplierDto>> DeleteSupplier(int id)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "Request to SuppliersController, DeleteSupplier action");

            var supplierToDelete = await _supplierService.GetSupplierWithIdAsync(id);

            if (supplierToDelete == null)
                return NotFound();

            var deleteResult = await _supplierService.DeleteSupplier(supplierToDelete);

            if (deleteResult)
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }
        }
    }
}
