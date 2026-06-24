using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchProgramWithVenue
{
    public OrchProgram Program { get; set; } = new OrchProgram();
    public List<OrchVenue> Venues { get; set; } = new List<OrchVenue>();
}

public class OrchProgramsWithShowsResult
{
    public int Page { get; set; } = 0;
    public ICollection<OrchProgramWithVenue> ProgramWithVenues { get; set; } = new List<OrchProgramWithVenue>();
}