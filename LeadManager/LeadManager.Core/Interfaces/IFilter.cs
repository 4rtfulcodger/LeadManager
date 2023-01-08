namespace LeadManager.Core.Interfaces
{
    public interface IFilter
    {
        DateTime FromCreatedDate { get; set; }
        bool IncludeAttributes { get; set; }
        string OrderBy { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        DateTime ToCreatedDate { get; set; }
    }
}