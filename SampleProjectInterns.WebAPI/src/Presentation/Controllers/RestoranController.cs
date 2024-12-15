using Application.CQRS.Restorans;
using Application.Dtos.Restorans.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified")]
	public class RestoranController : ControllerBase
	{
		private readonly ISender _sender;

		public RestoranController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] RestoranCreateDto restoran)
		{
			return Ok(await _sender.Send(new CreateRestoranCommand(restoran)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, [FromForm]RestoranUpdateDto restoran)
		{
			return Ok(await _sender.Send(new UpdateRestoranCommand(restoran, id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteRestoranCommand(id)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetRestoransQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetRestoranByIdQuery(id)));
		}
		[Route("GetByKategoriId/{id}")]  
		[HttpGet]
		public async Task<IActionResult> GetByKategoriId(long id)
		{
			
			return Ok(await _sender.Send(new GetRestoransByKategoriIdQuery(id)));
		}
	}
}
