using AutoMapper;
using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Supplier;
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
        private readonly IApiEndpointHandler _apiEndpointHandler;

        public SuppliersController(ISupplierService supplierService,
            ILogger<FilesController> logger,
            IEmailService emailService,
            IMapper mapper,
            IApiEndpointHandler apiEndpointHandler)
        {
            _supplierService = supplierService ?? throw new ArgumentException(nameof(supplierService)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _mapper = mapper;
            _apiEndpointHandler = apiEndpointHandler;
        }


        [HttpGet()]
        public async Task<IActionResult> GetSuppliers()
        {
            var getResult = await _supplierService.GetSuppliersAsync();
            return _apiEndpointHandler.ReturnSearchResult<IEnumerable<Supplier>, SupplierDto[]>(getResult);            
        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierWithIdAsync(id);
            return _apiEndpointHandler.ReturnSearchResult<Supplier, SupplierDto>(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(SupplierForCreateDto supplierDto)
        {
            var newSupplier = _mapper.Map<Supplier>(supplierDto);

            return _apiEndpointHandler.ReturnCreateResult<SupplierDto>(await _supplierService.AddSupplierAsync(newSupplier),
                "GetSupplier",
                newSupplier.SupplierId.ToString(),
                newSupplier);
        }

        [HttpPatch("{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(JsonPatchDocument<SupplierForUpdateDto> patchDocument, int supplierId)
        {
            var supplierEntity = await _supplierService.GetSupplierWithIdAsync(supplierId);

            if (!_apiEndpointHandler.IsValidEntitySearchResult<Supplier>(supplierEntity))
                return BadRequest();            

            var supplierDto = _mapper.Map<SupplierForUpdateDto>(supplierEntity);
            patchDocument.ApplyTo(supplierDto);
            _mapper.Map(supplierDto, supplierEntity);

            return _apiEndpointHandler.ReturndUpdateResult(await _supplierService.UpdateSupplierAsync(supplierId));           

        }

        [HttpDelete("{id}", Name = "DeleteSupplier")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to SuppliersController, DeleteSupplier action");

            var supplierToDelete = await _supplierService.GetSupplierWithIdAsync(id);

            if (!_apiEndpointHandler.IsValidEntitySearchResult<Supplier>(supplierToDelete))
                return BadRequest();

            return _apiEndpointHandler.ReturnDeleteResult(await _supplierService.DeleteSupplier(supplierToDelete));
        }
    }
}
