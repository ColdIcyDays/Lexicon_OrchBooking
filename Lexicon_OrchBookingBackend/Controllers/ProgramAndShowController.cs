using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Controllers.DTOs;
using Lexicon_OrchBookingBackend.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            
            OrchProgram? foundProgram = _context.Programs.Any(program => program.Id == aRequest.VenueId) ? _context.Programs.FirstOrDefault(program => program.Id == aRequest.VenueId) : null;
            if (foundProgram == null)
            {
                return BadRequest("No program with id [" + aRequest.ProgramId + "]!");
            }

            /* TODO: Can probably remove the program/venue assigments and only assign the ids... */
            Show createdShow = new Show();
            createdShow.VenueId = aRequest.VenueId;
            createdShow.Venue = foundVenue;
            createdShow.ShowDate = DateTime.SpecifyKind(aRequest.ShowDate, DateTimeKind.Utc);
            createdShow.ProgramId = aRequest.ProgramId;
            createdShow.Program = foundProgram;

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
            
            /* TODO: Make table for ticket prices, they need a key for the venue they belong to. */
            List<TicketPrice> ticketPrices = new List<TicketPrice>();
            foreach (var ticketPrice in aRequest.TicketPrices)
            {
                TicketPrice price = new TicketPrice();
                price.TicketCost = ticketPrice.TicketCost;
                price.TicketName = ticketPrice.TicketName;
                ticketPrices.Add(price);
            }

            await _context.Venues.AddAsync(createdOrchVenue);
            bool success = await _context.SaveChangesAsync() > 0;

            if (!success)
            {


                return StatusCode(500, "Failed to add venue!");
            }
            
            foreach (var price in ticketPrices)
            {
                price.OrchVenueId = createdOrchVenue.Id;
            }
            
            await _context.TicketPrices.AddRangeAsync(ticketPrices);
            bool savedTicketPrices = await _context.SaveChangesAsync() > 0;

            if (!savedTicketPrices)
            {
                _context.Venues.Remove(createdOrchVenue);
                await _context.SaveChangesAsync();
                
                return StatusCode(500, "Failed to add ticket prices...");
            }



            createdOrchVenue.TicketPrices = ticketPrices;
            
            return Ok("Successfully added venue!");
        }

        
        [HttpGet]
        [Route("GetPrograms")]
        public async Task<OrchGetProgramsResult> GetPrograms([FromQuery] int PerPage = -1, [FromQuery] int Page = 0, [FromQuery] string SortMethod = "")
        {
            OrchGetProgramsResult result = new OrchGetProgramsResult();
            
            if (PerPage <= 0)
            {
                result.Page = 0;
                result.FoundPrograms = _context.Programs.OrderByDescending(b => b.Id).ToList();
                return result;
            }

            Func<OrchProgram, int> idSort = program => program.Id;
            switch (SortMethod.ToLower())
            {
                /* Add sorts here... */
                default: // Sort by showdate.
                    result.FoundPrograms = await PaginationSet<OrchProgram, object, int>(_context.Programs, PerPage, Page, null, idSort).ToListAsync();
                    break;
            }
            
            return result;
        }
        
        [HttpGet]
        [Route("GetVenues")]
        public async Task<OrchGetVenuesResult> GetVenues()
        {
            OrchGetVenuesResult result = new OrchGetVenuesResult();
            
            result.Venues = await _context.Venues.Include(v => v.TicketPrices).OrderByDescending(v => v.Id).ToListAsync();

            return result;
        }
        
        [HttpGet]
        [Route("GetShows")]
        public async Task<OrchGetShowsResult> GetShows([FromQuery] int PerPage = -1, [FromQuery] int Page = 0, [FromQuery] string SortMethod = "")
        {
            OrchGetShowsResult result = new OrchGetShowsResult();
            
            if (PerPage <= 0)
            {
                result.Page = 0;
                result.FoundShows = await _context.Shows.OrderBy(b => b.ShowDate).ThenBy(b => b.Id)
                    .Include(show => show.Venue)
                    .Include(show => show.Program)
                    .ToListAsync();
                return result;
            }

            Func<Show, int> idSort = show => show.Id;
            switch (SortMethod.ToLower())
            {
                /* Add sorts here... */
                default: // Sort by showdate.
                    result.FoundShows = await PaginationSet(_context.Shows, PerPage, Page, show => show.ShowDate, idSort)
                        .Include(show => show.Venue)
                        .Include(show => show.Program)
                        .ToListAsync();
                    break;
            }
            
            return result;
        }


        private static IQueryable<T> PaginationSet<T, TSortType, TIdType>(DbSet<T> aDbSet, int aPerPage, int aPage, Func<T, TSortType>? aSortMethod, Func<T, TIdType> aSortById) where T : class
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
            
            return orderedEntities.Skip(safePage * safePerPage).Take(safePerPage);
        }
        
    }
}
