using System.Security.Claims;
using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Controllers.DTOs;
using Lexicon_OrchBookingBackend.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_OrchBookingBackend.Controllers;

[ApiController]
[Route("Account/[controller]")]
public class AuthController : ControllerBase
{
    private Lexicon_OrchBookingBackendContext _context;
    private readonly UserManager<Lexicon_OrchBookingBackendUser> _userManager;
    private readonly SignInManager<Lexicon_OrchBookingBackendUser> _signInManager;
    public AuthController(Lexicon_OrchBookingBackendContext aContext, UserManager<Lexicon_OrchBookingBackendUser> aUserManager, SignInManager<Lexicon_OrchBookingBackendUser> signInManager)
    {
        _userManager = aUserManager;
        _signInManager = signInManager;
        _context = aContext;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromForm] OrchRegisterRequest aRequest)
    {
        var identityUser = new Lexicon_OrchBookingBackendUser();
        identityUser.Email = aRequest.Email;
        identityUser.UserName = aRequest.Username;
        
        UserData userData = new UserData();
        userData.DisplayName = identityUser.UserName;
            
        await _context.UserDatas.AddAsync(userData);
        await _context.SaveChangesAsync();

        identityUser.UserDataId = userData.Id;

        var identityResult = await _userManager.CreateAsync(identityUser, aRequest.Password);

        if (identityResult.Succeeded)
        {
            identityResult = await _userManager.AddToRoleAsync(identityUser, "RegularUser");
            if (identityResult.Succeeded)
            {
                return Ok("Success! Please login!");
            }
            
            return BadRequest("Uh oh, User creation success, but failed adding role!");
        }
        
        _context.UserDatas.Remove(userData);
        await _context.SaveChangesAsync();

        return BadRequest("Uh oh, something went wrong! Result: " + identityResult.ToString());
    }

    [HttpGet]
    [Route("CheckAuth")]
    public async Task<IActionResult> CheckAuth()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Ok("You are authed! Heartbeat wow!");
        }
        
        return BadRequest("You are NOT authed! HEARTBEAT FAILED SAD");
    }
    
    [HttpGet]
    [Route("AccountInfo")]
    public async Task<ActionResult<OrchInfoData>> GetAccountInfo()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            OrchInfoData data = new OrchInfoData();
            var foundUser = await _userManager.GetUserAsync(User);

            if (foundUser == null)
            {
                return BadRequest("You are NOT authed! HEARTBEAT FAILED SAD");
            }
            data.Email = foundUser.Email;
            data.Username = foundUser.UserName;
            data.Roles = _userManager.GetRolesAsync(foundUser).Result.ToArray();
            
            data.Data = _context.UserDatas.First(v => v.Id == foundUser.UserDataId);
            
            return Ok(data);
        }
        
        return BadRequest("You are NOT authed! HEARTBEAT FAILED SAD");
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromForm] OrchLoginRequest aRequest)
    {
        // Note: Currently locout failure is always false... Track that on the api side?
        Lexicon_OrchBookingBackendUser? user = await _userManager.FindByNameAsync(aRequest.UserName);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(aRequest.UserName);
        }

        if (user == null)
        {
            return BadRequest("Failed to find user with username: '" + aRequest.UserName + "'");
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, aRequest.Password, aRequest.RememberMe);
      
        if (result.Succeeded)
        {
            var claims = new List<Claim>
            {
                new Claim("Username", aRequest.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await _signInManager.SignInAsync(user, aRequest.RememberMe);
            
            return Ok("You are authed!");
        }
        
        return BadRequest("Password check failed: '" + result + "'");
    }

    [HttpPost]
    [Route("Logout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
    
    [HttpPost]
    [Authorize (Roles = "Admin")]
    [Route("ModifyUserRole")]
    public async Task<IActionResult> ModifyUserRole(OrchModifyUsersRole aRequest)
    {
        return Ok("Here are your claims! ");
    }
}