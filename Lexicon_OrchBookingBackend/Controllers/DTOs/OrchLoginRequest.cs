namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchLoginRequest
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required bool RememberMe { get; init; }
}