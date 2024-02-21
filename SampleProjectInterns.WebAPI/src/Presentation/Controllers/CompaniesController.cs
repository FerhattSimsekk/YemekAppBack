using Application.CQRS.Companies;
using Application.Dtos.Companies.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "verified", Roles = "admin")]
public class CompaniesController : ControllerBase
{
    private readonly ISender _sender;

    public CompaniesController(ISender sender)
    {
        _sender = sender;
    }




    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CompanyCreateDto company)
    {
        return Ok(await _sender.Send(new CreateCompanyCommand(company)));
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromForm] CompanyUpdateDto company)
    {
        return Ok(await _sender.Send(new UpdateCompanyCommand(company,id)));
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        return Ok(await _sender.Send(new GetCompanyByIdQuery( id)));
    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _sender.Send(new GetCompaniesQuery()));
    }




    [HttpDelete("{id}")] 
    public async Task<IActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteCompanyCommand(id));
        return Ok();
    }
}
