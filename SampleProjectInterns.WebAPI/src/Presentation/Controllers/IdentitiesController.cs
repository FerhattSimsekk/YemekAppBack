using Application.CQRS.Identities;
using Application.Dtos.Identities.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "verified", Roles = "admin,moderator")]
public class IdentitiesController : ControllerBase
{
    private readonly ISender _sender;

    public IdentitiesController(ISender sender)
    {
        _sender = sender;
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(IdentityCreateDto identity)
    {
        var newUser = await _sender.Send(new CreateIdentityCommand(identity)); 
        var result = new { user = newUser };

        return Ok(result);
    }
}
