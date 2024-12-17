using Application.Dtos.Identities.Response;
using Application.Dtos.Yorumlar.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Yorumlar
{
	public record GetYorumlarsQuery(long restoranId) : IRequest<List<YorumDto>>;

	public class GetYorumlarQueryHandler : IRequestHandler<GetYorumlarsQuery, List<YorumDto>>
	{
		private readonly IWebDbContext _webDbContext;

		public GetYorumlarQueryHandler(IWebDbContext webDbContext)
		{
			_webDbContext = webDbContext;
		}

		public async Task<List<YorumDto>> Handle(GetYorumlarsQuery request, CancellationToken cancellationToken)
		{
			var yorumlar = await _webDbContext.Siparisler
		.Where(s => s.RestoranId == request.restoranId)
		.SelectMany(s => s.Yorumlar)
		.AsNoTracking()
		.Include(y => y.Identity) // Include user info (Identity)
		.ToListAsync(cancellationToken);

			return yorumlar.Select(y => new YorumDto(
				y.Id,
				new IdentityDto(
					y.Identity.Email,

					y.Identity.Name,
					y.Identity.LastName,
					y.Identity.Type.ToString()
				),
				y.Derecelendirme,
				y.YorumMetni,
				y.Status,
				y.CreatedAt,
				y.UpdatedAt
			)).ToList();
		}
	}
}
