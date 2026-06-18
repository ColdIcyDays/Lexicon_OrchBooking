using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchGetShowsResult
{
    public int Page { get; set; } = 0;
    public List<Show> FoundShows { get; set; } = new List<Show>();
}