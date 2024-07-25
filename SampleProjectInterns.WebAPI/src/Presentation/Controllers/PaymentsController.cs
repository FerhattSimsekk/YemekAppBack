using Application.CQRS.Employees;
using Application.CQRS.Payments;
using Application.Dtos.Employees.Request;
using Application.Dtos.Payments.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "verified", Roles = "admin")]
public class PaymentController : ControllerBase
{
	private readonly ISender _sender;

	public PaymentController(ISender sender)
	{
		_sender = sender;
	}
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] PaymentCreateDto payment)
	{
		return Ok(await _sender.Send(new CreatePaymentCommand(payment)));
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(long id, PaymentUpdateDto payment)
	{
		return Ok(await _sender.Send(new UpdatePaymentCommand(payment, id)));
	}
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(long id)
	{
		return Ok(await _sender.Send(new DeletePaymentCommand(id)));
	}
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		return Ok(await _sender.Send(new GetPaymentsQuery()));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await _sender.Send(new GetPaymentByIdQuery(id)));
	}


}