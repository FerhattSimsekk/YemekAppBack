using Application.CQRS.Restorans;
using Application.CQRS.SiparisDetays;
using Application.CQRS.Siparisler;
using Application.CQRS.Uruns;
using Application.Dtos.Restorans.Request;
using Application.Dtos.SiparisDetays;
using Application.Dtos.Siparisler.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "verified")]
	public class SiparisController : ControllerBase
	{
		private readonly ISender _sender;

		public SiparisController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		[Route("CreateSiparis")]
		public async Task<IActionResult> CreateSiparis([FromBody] SiparisCreateDto siparisCreateRequest)
		{
			if (siparisCreateRequest == null || siparisCreateRequest.SiparisDetaylari == null || siparisCreateRequest.SiparisDetaylari.Count == 0)
			{
				return BadRequest("Sipariş bilgileri eksik.");
			}

			// SiparisDetaylari'ni List<SiparisDetayCreateDto> türüne dönüştürme
			var siparisDetayCreateDtoList = siparisCreateRequest.SiparisDetaylari
				.Select(item => new SiparisDetayCreateDto
				{
					UrunId = item.UrunId,
					Adet = item.Adet,
					Fiyat = item.Fiyat
				})
				.ToList();

			var command = new CreateSiparisDetayCommand(
				SiparisDetay: siparisDetayCreateDtoList,
				RestoranId: siparisCreateRequest.RestoranId,
				AdresId:siparisCreateRequest.AdresId
			);

			try
			{
				var result = await _sender.Send(command);
				if (result)
				{
					return Ok("Sipariş başarıyla oluşturuldu.");
				}
				else
				{
					return StatusCode(500, "Sipariş oluşturulurken bir hata oluştu.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Hata: {ex.Message}");
			}
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetSiparislerQuery()));
		}
		//[HttpGet("{id}")]
		//public async Task<IActionResult> Get(long id)
		//{
		//	//return Ok(await _sender.Send(new GetSiparisByIdQuery(id)));
		//}
	}
}
