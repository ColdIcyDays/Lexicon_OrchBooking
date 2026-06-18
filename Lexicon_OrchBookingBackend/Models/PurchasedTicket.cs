using System.ComponentModel.DataAnnotations.Schema;

namespace Lexicon_OrchBookingBackend.Models;

public class PurchasedTicket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ShowId { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime PurchaseDate { get; set; }
    public TicketPrice Price { get; set; }
}