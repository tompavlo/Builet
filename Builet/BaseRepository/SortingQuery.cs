namespace Builet.BaseRepository;

public class SortingQuery
{
    public string SortBy { get; set; } = "Id";
    
    public SortOrder SortDirection { get; set; } = SortOrder.Asc;
}