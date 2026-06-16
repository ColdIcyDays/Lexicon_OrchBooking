using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lexicon_OrchBookingBackend.Areas.Identity.Data;

namespace Lexicon_OrchBookingBackend.Models;

public class UserData
{
    [Key]
    public int Id { get; set; }
    [Required]
    [ForeignKey(nameof(Lexicon_OrchBookingBackendUser))]
    public int UserId { get; set; }
    
    [Required]
    public string DisplayName { get; set; }
}