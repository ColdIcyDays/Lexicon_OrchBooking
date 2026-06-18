using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchUploadVenueRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int MaxSeating { get; set; }
    public OrchVenueTicketPriceRequest[] TicketPrices { get; set; }
}