using Application.Dtos.RestoranKategorileri.Response;
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
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.RestoranKategorileri
{
	public record GetRestoranKategorileriQuery() : IRequest<List<RestoranKategoriDTO>>;

	public class GetRestoranKategorileriQueryhandler : IRequestHandler<GetRestoranKategorileriQuery, List<RestoranKategoriDTO>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetRestoranKategorileriQueryhandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<RestoranKategoriDTO>> Handle(GetRestoranKategorileriQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");



			var kategorim = await _webDbContext.KategoriRestoranlar
				.AsNoTracking()
				.Select(kategorim => kategorim.MaptoRestoranKategoriDto())
				.ToListAsync(cancellationToken);

			return kategorim;
		}
	}
}
