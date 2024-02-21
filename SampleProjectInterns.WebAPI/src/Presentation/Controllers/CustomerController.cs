using Application.CQRS.Companies;
using Application.CQRS.Customers;
using Application.Dtos.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "verified", Roles = "admin")]


public class CustomerController : ControllerBase
{
    private readonly ISender _sender;

    public CustomerController(ISender sender)
    {
        _sender = sender;
    }




    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CustomerCreateDto customer)
    {
        return Ok(await _sender.Send(new CreateCustomerCommand(customer)));
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id,CustomerUpdateDto customer)
    {
        return Ok(await _sender.Send(new UpdateCustomerCommand(customer,id)));
    }





    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _sender.Send(new DeleteCustomerCommand(id));
        return Ok();
    }




    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        return Ok(await _sender.Send(new GetCustomerByIdQuery(id)));
    }

}
