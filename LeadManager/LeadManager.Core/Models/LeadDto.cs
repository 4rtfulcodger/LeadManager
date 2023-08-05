﻿using LeadManager.Core.Entities;
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

    public class LeadDto
    {
        public int Id { get; set; }

        public SourceDto? Source { get; set; }

        public SupplierDto? Supplier { get; set; }

        public List<ContactDto>? Contacts { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LeadTypeForCreateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class LeadForCreateDto
    {
        public int LeadTypeId { get; set; }

        public int SourceId { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<ContactForCreateDto> Contacts { get; set; }
    }

    public class LeadForUpdateDto
    {
        public int SourceId { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
