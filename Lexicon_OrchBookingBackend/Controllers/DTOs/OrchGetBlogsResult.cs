using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchGetBlogsResult
{
    public int Page { get; set; } = 0;
    public List<Blog> FoundBlogs { get; set; } = new List<Blog>();
}