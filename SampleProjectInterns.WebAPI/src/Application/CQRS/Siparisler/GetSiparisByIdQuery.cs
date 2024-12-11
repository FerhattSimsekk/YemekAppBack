using Application.Dtos.Siparisler.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Siparisler
{
	public record GetSiparisByIdQuery(long SiparisId) : IRequest<SiparisDto>;

	public class GetSiparisByIdQueryHandler : IRequestHandler<GetSiparisByIdQuery, SiparisDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetSiparisByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<SiparisDto> Handle(GetSiparisByIdQuery request, CancellationToken cancellationToken)
		{
			// Kullanıcı kimliğini doğrula
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			// Siparişi veritabanından getir
			var siparis = await _webDbContext.Siparisler
				.Include(s => s.SiparisDetaylari) // Sipariş detaylarını da getir
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.Id == request.SiparisId, cancellationToken)
				?? throw new NotFoundException("Siparis not found", "Siparis");

			// DTO olarak döndür
			return siparis.MapToSiparisDto();
		}
	}
}
