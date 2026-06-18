using System.Numerics;
using System.Runtime.InteropServices.JavaScript;

namespace Lexicon_OrchBookingBackend.Models;

public class TicketPrice
{
    public int Id { get; set; }
    public string TicketName { get; set; }
    public long TicketCost { get; set; }
}