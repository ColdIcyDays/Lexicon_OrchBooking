using Microsoft.AspNetCore.Identity.Data;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchRegisterRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// The user's username.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }
}