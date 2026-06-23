using Lexicon_OrchBookingBackend.Models;

namespace Lexicon_OrchBookingBackend.Controllers.DTOs;

public class OrchInfoData
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }

    public UserData Data { get; set; }
}