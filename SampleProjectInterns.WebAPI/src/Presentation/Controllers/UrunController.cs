using Application.CQRS.Restorans;
using Application.CQRS.Uruns;
using Application.Dtos.Restorans.Request;
using Application.Dtos.Uruns.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProjectInterns.Entities;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified")]
	public class UrunController : ControllerBase
	{
		private readonly ISender _sender;

		public UrunController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UrunCreateDto urun)
		{
			return Ok(await _sender.Send(new CreateUrunCommand(urun)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, UrunUpdateDto urun)
		{
			return Ok(await _sender.Send(new UpdateUrunCommand(urun, id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteUrunCommand(id)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetUrunsQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetUrunByIdQuery(id)));
		}
		[HttpGet("restoran/{restoranId}")]
		public async Task<IActionResult> GetByRestoranId(long restoranId)
		{
			return Ok(await _sender.Send(new GetUrunsByRestoranIdQuery(restoranId)));
		}
	}
}
