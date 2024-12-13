using Application.CQRS.Adresler;
using Application.CQRS.Uruns;
using Application.Dtos.Adresler.Request;
using Application.Dtos.Uruns.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified")]
	public class AdresController : ControllerBase
	{
		private readonly ISender _sender;

		public AdresController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] AdresCreateDto adres)
		{
			return Ok(await _sender.Send(new CreateAdresCommand(adres)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, [FromBody] AdresUpdateDto adres)
		{
			return Ok(await _sender.Send(new UpdateAdresCommand(adres, id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteAdresCommand(id)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetAdreslerQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetAdresByIdQuery(id)));
		}
		
	}
}

