namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchUploadBlogRequest
{
    public required string ContentTitle { get; set; }
    public required string ContentBody { get; set; }
    public required string BlogImage { get; set; }
}