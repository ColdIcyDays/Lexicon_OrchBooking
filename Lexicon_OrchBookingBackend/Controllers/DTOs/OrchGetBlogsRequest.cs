namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchGetBlogsRequest
{
    public bool UsePagination { get; set; } = true;
    public int Page { get; set; } = 0;
    public int PerPage { get; set; } = 1;
    public string SortMethod { get; set; } = "date";
}