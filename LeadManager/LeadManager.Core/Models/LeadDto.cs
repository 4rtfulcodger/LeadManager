using LeadManager.Core.Entities;

namespace LeadManager.Core.ViewModels
{
    public class LeadDto
    {
        public int Id { get; set; }

        public SourceDto? Source { get; set; }

        public SupplierDto? Supplier { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LeadForCreateDto
    {
        public int SourceId { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LeadForUpdateDto
    {
        public int SourceId { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
