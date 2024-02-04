using LeadManager.Core.Entities;
using LeadManager.Core.Models;

namespace LeadManager.Core.ViewModels
{

    public class LeadTypeDto
    {
        public int LeadTypeId { get; set; }

        public Guid LeadTypeReference { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class LeadAttributeDto
    {
        public int LeadAttributeId { get; set; }

        public int LeadTypeId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class LeadDto
    {
        public int Id { get; set; }

        public SourceDto? Source { get; set; }

        public SupplierDto? Supplier { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<ContactDto>? Contacts { get; set; }

        public List<LeadAttributeValueDto>? LeadAttributeValues { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LeadTypeForCreateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class LeadAttributeForCreateDto
    {
        public int LeadTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class LeadAttributeValueDto
    {
        //public int LeadAttributeId { get; set; }

        public string LeadAttributeName { get; set; }

        public string AttributeValue { get; set; }
    }

    public class LeadAttributeValueForCreateDto
    {
        public int LeadAttributeId { get; set; }

        public string LeadAttributeName { get; set; }

        public string AttributeValue { get; set; }
    }

    public class LeadForCreateDto
    {
        public int LeadTypeId { get; set; }

        public string LeadTypeRef { get; set; }

        public int SourceId { get; set; }

        public string SourceRef { get; set; }

        public int SupplierId { get; set; }

        public string SupplierRef { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<ContactForCreateDto> Contacts { get; set; }

        public List<LeadAttributeValueForCreateDto> LeadAttributeValues { get; set; }
    }

    public class LeadTypeForUpdateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class LeadAttributeForUpdateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class LeadForUpdateDto
    {
        public int SourceId { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
