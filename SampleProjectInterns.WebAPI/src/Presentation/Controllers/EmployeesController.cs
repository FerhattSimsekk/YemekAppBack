using Application.CQRS.Employees;
using Application.CQRS.Payments;
using Application.Dtos.Employees.Request;
using Application.Dtos.Payments.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified", Roles = "admin")]

	public class EmployeesController : ControllerBase
	{

		private readonly ISender _sender;

		public EmployeesController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] EmployeeCreateDto employee)
		{
			return Ok(await _sender.Send(new CreateEmployeeCommand(employee)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, EmployeeUpdateDto employee)
		{
			return Ok(await _sender.Send(new UpdateEmployeeCommand(employee, id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteEmployeeCommand(id)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetEmployeesQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetEmployeeByIdQuery(id)));
		}

	}
}
