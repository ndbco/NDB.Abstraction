namespace NDB.Abstraction.Requests;

public class ListRequest
{
    public List<FilterRequest> Filters { get; set; } = new();
    public List<SortRequest> Sorts { get; set; } = new();
    public PagingRequest Paging { get; set; } = new();
}
