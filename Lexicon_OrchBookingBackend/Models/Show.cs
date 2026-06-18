using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexicon_OrchBookingBackend.Models;

public class Show
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(OrchProgram))]
    public int ProgramId { get; set; }
    
    [ForeignKey(nameof(OrchVenue))]
    public int VenueId { get; set; }
    public OrchVenue Venue { get; set; }

    [Required]
    public DateTime ShowDate { get; set; }

    public ICollection<PurchasedTicket> PurchasedTickets { get; set; }
}