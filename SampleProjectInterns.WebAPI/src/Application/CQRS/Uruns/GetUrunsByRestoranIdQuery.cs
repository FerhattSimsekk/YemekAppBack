using Application.Dtos.Uruns.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Uruns
{
	public record GetUrunsByRestoranIdQuery(long RestoranId) : IRequest<List<UrunDto>>;

	public class GetUrunsByRestoranIdHandler : IRequestHandler<GetUrunsByRestoranIdQuery, List<UrunDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetUrunsByRestoranIdHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<UrunDto>> Handle(GetUrunsByRestoranIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

		

			var uruns = await _webDbContext.Urunler
				.AsNoTracking()
				.Where(uruns => uruns.RestoranId == request.RestoranId) // RestoranId'ye göre filtreleme
				.OrderByDescending(order => order.CreatedAt)
				.Select(uruns => uruns.MapToUrunDto())
				.ToListAsync(cancellationToken);

			return uruns;
		}
	}
}
