using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lexicon_OrchBookingBackend.Areas.Identity.Data;

namespace Lexicon_OrchBookingBackend.Models;

public class Blog
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Lexicon_OrchBookingBackendUser))]
    public string WriterId { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; }
    
    [Required]
    public string ContentTitle { get; set; }
    public string ContentBody { get; set; }
    
    public string[] Images { get; set; }
}