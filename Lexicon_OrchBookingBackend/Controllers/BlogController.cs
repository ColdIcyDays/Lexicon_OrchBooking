using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Controllers.DTOs;
using Lexicon_OrchBookingBackend.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_OrchBookingBackend.Controllers //  
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private Lexicon_OrchBookingBackendContext _context;
        private UserManager<Lexicon_OrchBookingBackendUser> _userManager;
        public BlogController(Lexicon_OrchBookingBackendContext aContext, UserManager<Lexicon_OrchBookingBackendUser> aUserManager)
        {
            _context = aContext;
            _userManager = aUserManager;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Blogwriter")]
        [Route("UploadBlog")]
        public async Task<ActionResult> UploadBlog([FromForm] OrchUploadBlogRequest aRequest)
        {
            Blog blog = new Blog();
            blog.ContentTitle = aRequest.ContentTitle;
            blog.ContentBody = aRequest.ContentBody;
            blog.Images = [aRequest.BlogImage];

            string? writerId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(writerId))
            {
                blog.WriterId = writerId;
            }

            await _context.Blogs.AddAsync(blog);
            bool success = await _context.SaveChangesAsync() > 0;
            
            return success ? Ok("Successfully added blog!") : BadRequest("Failed to add blog!");
        }

        [HttpGet]
        [Route("GetBlogs")]
        public async Task<OrchGetBlogsResult> GetBlogs([FromQuery] int PerPage = -1, [FromQuery] int Page = 0, [FromQuery] string SortMethod = "")
        {
            OrchGetBlogsResult result = new OrchGetBlogsResult();

            if (PerPage <= 0)
            {
                result.Page = 0;
                result.FoundBlogs = _context.Blogs.OrderBy(b => b.DateCreated).ThenBy(b => b.Id).ToList();
                return result;
            }
            
            IQueryable<Blog> orderedBlogs;
            switch (SortMethod.ToLower())
            {
                /* Add sorts here... */
                default:
                    orderedBlogs = _context.Blogs.OrderBy(b => b.DateCreated).ThenBy(b => b.Id);
                    break;
            }

            int totalBlogs = _context.Blogs.Count();
            
            int safePerPage = Math.Min(PerPage, totalBlogs);
            int safePage = Math.Min((int)Math.Ceiling((double)totalBlogs / (double)safePerPage), Page);
                
            result.FoundBlogs = orderedBlogs.Skip(safePage * safePerPage).Take(safePerPage).ToList();
            return result;
        }
    }
}
