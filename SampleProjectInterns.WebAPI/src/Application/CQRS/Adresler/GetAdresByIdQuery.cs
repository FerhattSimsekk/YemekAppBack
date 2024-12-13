using Application.Dtos.Adresler.Response;
using Application.Dtos.Uruns.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
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
	public record GetAdresByIdQuery(long Id) : IRequest<AdresDto>;

	public class GetAdresByIdQueryHandler : IRequestHandler<GetAdresByIdQuery, AdresDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetAdresByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<AdresDto> Handle(GetAdresByIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
			 .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			 ?? throw new Exception("User not found");





			var adres = await _webDbContext.Adresler.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
			   ?? throw new NotFoundException($"Urun not found", "adres");



			return adres.MapToAdresDto();
		}
	}
}
