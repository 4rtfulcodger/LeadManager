using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Core.ViewModels;
using LeadManager.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ILogger<SupplierService> _logger;
        public ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository,
            ILogger<SupplierService> logger)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentException(nameof(supplierRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            supplier.SupplierRef = Guid.NewGuid();
            return await _supplierRepository.AddSupplierAsync(supplier);
        }

        public async Task<bool> DeleteSupplier(Supplier supplier)
        {
            return await _supplierRepository.DeleteSupplier(supplier);
        }

        public async Task<Supplier?> GetSupplierWithIdAsync(int Id)
        {
            return await _supplierRepository.GetSupplierWithIdAsync(Id);
        }

        public async Task<Supplier?> GetSupplierByRefAsync(Guid SupplierRef)
        {
            return await _supplierRepository.GetSupplierWithRefAsync(SupplierRef);
        }

        public async Task<PagedList<Supplier>> GetSuppliersAsync(SupplierFilter filter)
        {
           return await _supplierRepository.GetSuppliersAsync(filter);
        }

        public async Task<bool> UpdateSupplierAsync(int Id)
        {
            return await _supplierRepository.UpdateSupplierAsync(Id);
        }
    }
}
