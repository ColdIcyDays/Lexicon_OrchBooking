namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchUploadShowRequest
{
    public int ProgramId { get; set; }
    public int VenueId { get; set; }
    public DateTime ShowDate { get; set; }
}