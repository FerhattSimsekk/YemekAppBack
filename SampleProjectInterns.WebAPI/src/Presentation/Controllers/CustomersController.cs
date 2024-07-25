using Application.CQRS.Customers;
using Application.CQRS.Employees;
using Application.CQRS.Payments;
using Application.Dtos.Customers.Request;
using Application.Dtos.Employees.Request;
using Application.Dtos.Payments.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProjectInterns.Entities;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified", Roles = "admin")]
	public class CustomersController : ControllerBase
	{
		private readonly ISender _sender;

		public CustomersController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CustomerCreateDto customer)
		{
			return Ok(await _sender.Send(new CreateCustomerCommand(customer)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, CustomerUpdateDto employee)
		{
			return Ok(await _sender.Send(new UpdateCustomerCommand(employee, id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteCustomerCommand(id)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetCustomersQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetCustomerByIdQuery(id)));
		}
	}
}
