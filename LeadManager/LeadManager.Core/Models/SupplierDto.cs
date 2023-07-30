namespace LeadManager.Core.ViewModels
{
    public class SupplierDto
    {
        public Guid SupplierRef { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
        
    public class SupplierForCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
    public class SupplierForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
