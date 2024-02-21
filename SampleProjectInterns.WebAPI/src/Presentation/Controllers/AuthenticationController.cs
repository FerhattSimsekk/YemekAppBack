using Application.CQRS.Identities;
using Application.Dtos.Identities.Request;
using SampleProjectInterns.WebAPI.Presentation.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ISender _sender;

    public AuthenticationController(JwtService jwtService, ISender sender)
    {
        _jwtService = jwtService;
        _sender = sender;
    }




    [HttpPost]
    public async Task<IActionResult> Verify(IdentityVerifyDto identity)
    {
        var verifiedUser = await _sender.Send(new IdentityAuthenticationQuery(identity));

        if (verifiedUser != null)
        {
            var result = new
            {
                token = _jwtService.GenerateToken(verifiedUser),
                user = verifiedUser
            };

            return Ok(result);
        }

        return Unauthorized();
    }






    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var verifiedUser = await _sender.Send(new GetIdentityQuery());

        if (verifiedUser != null)
        {
            var result = new
            { 
                user = verifiedUser
            };

            return Ok(result);
        }

        return Unauthorized();
    }
  
}
