using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchGetProgramsResult
{
    public int Page { get; set; } = 0;
    public List<OrchProgram> FoundPrograms { get; set; } = new List<OrchProgram>();
}