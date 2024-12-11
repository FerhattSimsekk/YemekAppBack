using Application.Dtos.Siparisler.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Siparisler
{
	public record GetSiparislerQuery() : IRequest<List<SiparisDto>>;

	public class GetSiparislerQueryHandler : IRequestHandler<GetSiparislerQuery, List<SiparisDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetSiparislerQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<SiparisDto>> Handle(GetSiparislerQuery request, CancellationToken cancellationToken)
		{
			// Kullanıcı kimliğini al
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			// Kullanıcının rolüne göre filtrele
			IQueryable<Siparis> siparisQuery = _webDbContext.Siparisler.AsNoTracking();

			if (identity.Type == AdminAuthorization.user)
			{
				// Kullanıcı kendi siparişlerini görsün
				siparisQuery = siparisQuery.Where(s => s.IdentityId == identity.Id);
			}
			else if (identity.Type == AdminAuthorization.moderator)
			{
				// Restoran sahibi, kendi restoranına ait siparişleri görsün
				siparisQuery = siparisQuery.Where(s => s.RestoranId == identity.RestoranId);
			}
			// Admin için herhangi bir filtre uygulanmaz, tüm siparişleri görebilir

			// Siparişleri çek ve DTO'ya dönüştür
			var siparisler = await siparisQuery
				.Include(s => s.SiparisDetaylari) // Sipariş detaylarını da dahil et
				.OrderByDescending(s => s.OlusturmaTarihi)
				.Select(s => s.MapToSiparisDto())
				.ToListAsync(cancellationToken);

			return siparisler;
		}
	}
}
