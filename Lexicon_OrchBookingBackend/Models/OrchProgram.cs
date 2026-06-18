using System.ComponentModel.DataAnnotations;

namespace Lexicon_OrchBookingBackend.Models;

public class OrchProgram
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int LengthInMinutes { get; set; }
}