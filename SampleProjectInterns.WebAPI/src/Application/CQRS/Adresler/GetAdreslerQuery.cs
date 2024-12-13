using Application.Dtos.Adresler.Response;
using Application.Dtos.Uruns.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Adresler
{
	public record GetAdreslerQuery() : IRequest<List<AdresDto>>;

	public class GetAdreslerQueryHandler : IRequestHandler<GetAdreslerQuery, List<AdresDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetAdreslerQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<AdresDto>> Handle(GetAdreslerQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");



			var adresler = await _webDbContext.Adresler
				.AsNoTracking()
				.OrderByDescending(order => order.CreatedAt)
				.Where(x=>x.Status==SampleProjectInterns.Entities.Common.Enums.Status.approved)
				.Select(uruns => uruns.MapToAdresDto())
				.ToListAsync(cancellationToken);

			return adresler;
		}
	}
}
