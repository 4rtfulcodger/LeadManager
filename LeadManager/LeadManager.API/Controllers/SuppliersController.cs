using AutoMapper;
using LeadManager.API.Models;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public ILeadInfoRepository _leadInfoRepository;

        public SuppliersController(ILeadInfoRepository leadInfoRepository,
            ILogger<FilesController> logger,
            IEmailService emailService,
            IMapper mapper)
        {
            _leadInfoRepository = leadInfoRepository ?? throw new ArgumentException(nameof(leadInfoRepository)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _mapper = mapper;
        }


        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            return Ok(_mapper.Map<SupplierDto[]>(await _leadInfoRepository.GetSuppliersAsync()));
        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSupplier(int id)
        {
            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier(SupplierForCreateDto supplierDto)
        {
            var newSupplier = _mapper.Map<Supplier>(supplierDto);
            bool addLeadSuccess = await _leadInfoRepository.AddSupplierAsync(newSupplier);

            return CreatedAtRoute("GetSource", new { id = newSupplier.SupplierId }, _mapper.Map<SupplierDto>(newSupplier));

        }

        [HttpDelete("{id}", Name = "DeleteSupplier")]
        public async Task<ActionResult<SupplierDto>> DeleteSupplier(int id)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "Request to SuppliersController, DeleteSupplier action");

            var supplierToDelete = await _leadInfoRepository.GetSupplierWithIdAsync(id);

            if (supplierToDelete == null)
                return NotFound();

            var deleteResult = await _leadInfoRepository.DeleteSupplier(supplierToDelete);

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
