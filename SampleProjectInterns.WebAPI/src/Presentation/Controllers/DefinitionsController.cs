
using Application.CQRS.Definitions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "verified", Roles = "admin,moderator,lawyer,accounting,secretary,user")]
public class DefinitionsController : ControllerBase
{
    private readonly ISender _sender;

    public DefinitionsController(ISender sender)
    {
        _sender = sender;
    }



    [HttpGet("Cities")]
    public async Task<IActionResult> Cities()
    {
        var cityDto = await _sender.Send(new GetCitiesQuery());
        return Ok(cityDto);
    }



    [HttpGet("Counties/{CityId}")]
    public async Task<IActionResult> Counties(int CityId)
    {
        var districtDto = await _sender.Send(new GetCountiesQuery(CityId));
        return Ok(districtDto);
    }



    //[HttpGet("Companies")]
    //public async Task<IActionResult> Companies()
    //{
    //    var companyDto = await _sender.Send(new GetCompanyIdPageQuery());
    //    return Ok(companyDto);
    //}
}
