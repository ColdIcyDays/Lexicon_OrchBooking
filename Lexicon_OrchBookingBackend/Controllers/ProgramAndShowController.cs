using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Controllers.DTOs;
using Lexicon_OrchBookingBackend.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_OrchBookingBackend.Controllers //  
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private Lexicon_OrchBookingBackendContext _context;
        private UserManager<Lexicon_OrchBookingBackendUser> _userManager;
        public ShowController(Lexicon_OrchBookingBackendContext aContext, UserManager<Lexicon_OrchBookingBackendUser> aUserManager)
        {
            _context = aContext;
            _userManager = aUserManager;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Showmanager")]
        [Route("UploadProgram")]
        public async Task<ActionResult> UploadProgram([FromForm] OrchUploadProgramRequest aRequest)
        {
            OrchProgram program = new OrchProgram();
            program.Title = aRequest.Title;
            program.Description = aRequest.Description;
            program.LengthInMinutes = aRequest.LengthInMinutes;
            
            await _context.Programs.AddAsync(program);
            bool success = await _context.SaveChangesAsync() > 0;
            
            return success ? Ok("Added program!") : BadRequest("Failed to add program!");
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Showmanager")]
        [Route("UploadShow")]
        public async Task<ActionResult> UploadShow([FromForm] OrchUploadShowRequest aRequest)
        {
            if (!_context.Programs.Any(p => p.Id == aRequest.ProgramId))
            {
                return BadRequest("No program with id [" + aRequest.ProgramId + "]!");
            }

            OrchVenue? foundVenue = _context.Venues.Any(venue => venue.Id == aRequest.VenueId) ? _context.Venues.FirstOrDefault(venue => venue.Id == aRequest.VenueId) : null;
            if (foundVenue == null)
            {
                return BadRequest("No venue with id [" + aRequest.VenueId + "]!");
            }
            
            Show createdShow = new Show();
            createdShow.VenueId = aRequest.VenueId;
            createdShow.Venue = foundVenue;
            createdShow.ShowDate = DateTime.SpecifyKind(aRequest.ShowDate, DateTimeKind.Utc);
            createdShow.ProgramId = aRequest.ProgramId;

            await _context.Shows.AddAsync(createdShow);
            bool success = await _context.SaveChangesAsync() > 0;
            
            return success ? Ok("Successfully added show!") : BadRequest("Failed to add show!");
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Showmanager")]
        [Route("UploadVenue")]
        public async Task<ActionResult> UploadVenue([FromForm] OrchUploadVenueRequest aRequest)
        {
            OrchVenue createdOrchVenue = new OrchVenue();
            createdOrchVenue.Address = aRequest.Address;
            createdOrchVenue.Name = aRequest.Name;
            createdOrchVenue.MaxSeating = aRequest.MaxSeating;
            foreach (var ticketPrice in aRequest.TicketPrices)
            {
                TicketPrice price = new TicketPrice();
                price.TicketCost = ticketPrice.TicketCost;
                price.TicketName = ticketPrice.TicketName;
                createdOrchVenue.TicketPrices.Add(price);
            }

            await _context.Venues.AddAsync(createdOrchVenue);
            bool success = await _context.SaveChangesAsync() > 0;
            
            return success ? Ok("Successfully added venue!") : BadRequest("Failed to add venue!");
        }

        
        [HttpGet]
        [Route("GetPrograms")]
        public async Task<OrchGetProgramsResult> GetPrograms([FromQuery] int PerPage = -1, [FromQuery] int Page = 0, [FromQuery] string SortMethod = "")
        {
            OrchGetProgramsResult result = new OrchGetProgramsResult();
            
            if (PerPage <= 0)
            {
                result.Page = 0;
                result.FoundPrograms = _context.Programs.OrderBy(b => b.Id).ToList();
                return result;
            }

            Func<OrchProgram, int> idSort = program => program.Id;
            switch (SortMethod.ToLower())
            {
                /* Add sorts here... */
                default: // Sort by showdate.
                    result.FoundPrograms = PaginationSet<OrchProgram, object, int>(_context.Programs, PerPage, Page, null, idSort);
                    break;
            }
            
            return result;
        }
        
        [HttpGet]
        [Route("GetVenues")]
        public async Task<ICollection<OrchVenue>> GetVenues()
        {
            return await _context.Venues.ToListAsync();
        }
        
        [HttpGet]
        [Route("GetShows")]
        public async Task<OrchGetShowsResult> GetShows([FromQuery] int PerPage = -1, [FromQuery] int Page = 0, [FromQuery] string SortMethod = "")
        {
            OrchGetShowsResult result = new OrchGetShowsResult();
            
            if (PerPage <= 0)
            {
                result.Page = 0;
                result.FoundShows = _context.Shows.OrderBy(b => b.ShowDate).ThenBy(b => b.Id).ToList();
                return result;
            }

            Func<Show, int> idSort = show => show.Id;
            switch (SortMethod.ToLower())
            {
                /* Add sorts here... */
                default: // Sort by showdate.
                    result.FoundShows = PaginationSet(_context.Shows, PerPage, Page, show => show.ShowDate, idSort);
                    break;
            }
            
            return result;
        }


        private static List<T> PaginationSet<T, TSortType, TIdType>(DbSet<T> aDbSet, int aPerPage, int aPage, Func<T, TSortType>? aSortMethod, Func<T, TIdType> aSortById) where T : class
        {
            IQueryable<T> orderedEntities;
            if (aSortMethod != null)
            {
                orderedEntities = aDbSet.OrderBy(aSortMethod).ThenBy(aSortById).AsQueryable();
            }
            else
            {
                orderedEntities = aDbSet.OrderBy(aSortById).AsQueryable();
            }
            
            int totalBlogs = aDbSet.Count();
            
            int safePerPage = Math.Min(aPerPage, totalBlogs);
            int safePage = Math.Min((int)Math.Ceiling((double)totalBlogs / (double)safePerPage), aPage);
            
            return orderedEntities.Skip(safePage * safePerPage).Take(safePerPage).ToList();
        }
        
    }
}
