namespace LeadManager.Core.ViewModels
{
    public class SourceDto
    {
        public int Id { get; set; }
        public Guid SourceRef { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SourceForCreateDto
    {
        public Guid SourceRef { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
    public class SourceForUpdateDto
    {
        public Guid SourceRef { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
