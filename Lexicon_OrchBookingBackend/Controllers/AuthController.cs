using System.Security.Claims;
using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Controllers.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_OrchBookingBackend.Controllers;

[ApiController]
[Route("Account/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Lexicon_OrchBookingBackendUser> _userManager;
    private readonly SignInManager<Lexicon_OrchBookingBackendUser> _signInManager;
    public AuthController(UserManager<Lexicon_OrchBookingBackendUser> aUserManager, SignInManager<Lexicon_OrchBookingBackendUser> signInManager)
    {
        _userManager = aUserManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] OrchRegisterRequest aRequest)
    {
        var identityUser = new Lexicon_OrchBookingBackendUser();
        identityUser.Email = aRequest.Email;
        identityUser.UserName = aRequest.Username;

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

        return BadRequest("Uh oh, something went wrong! Result: " + identityResult.ToString());
    }

    [HttpGet]
    [Route("CheckAuth")]
    public async Task<IActionResult> CheckAuth()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Ok("You are authed!");
        }
        
        return BadRequest("You are NOT authed!");
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
    [Authorize (Roles = "Admin")]
    [Route("ModifyUserRole")]
    public async Task<IActionResult> ModifyUserRole([FromBody] OrchModifyUsersRole aRequest)
    {
        return Ok("Here are your claims! ");
    }
}