using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchGetVenuesResult
{
    public List<OrchVenue> Venues { get; set; }
}