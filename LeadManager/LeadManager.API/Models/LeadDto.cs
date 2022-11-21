namespace LeadManager.API.Models
{
    public class LeadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LeadForCreateDto
    {        
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
