using System.ComponentModel.DataAnnotations;

namespace Lexicon_OrchBookingBackend.Models;

public class OrchVenue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int MaxSeating { get; set; }
    public ICollection<TicketPrice> TicketPrices { get; set; }
}