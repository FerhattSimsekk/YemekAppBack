using Application.CQRS.Restorans;
using Application.CQRS.Yorumlar;
using Application.Dtos.Restorans.Request;
using Application.Dtos.Yorumlar.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class YorumController : ControllerBase
	{

		private readonly ISender _sender;

		public YorumController(ISender sender)
		{
			_sender = sender;
		}


		[HttpPost]
		public async Task<IActionResult> Post([FromBody] YorumCreateDto yorum)
		{
			return Ok(await _sender.Send(new CreateYorumCommand(yorum)));
		}

		[HttpGet("{id}")]

		public async Task<IActionResult> GetYorumlar( long id)
		{
			var result = await _sender.Send(new GetYorumlarsQuery(id));
			return Ok(result);
		}
	}
}
