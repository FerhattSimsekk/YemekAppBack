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

namespace Application.CQRS.Uruns
{
	public record GetUrunsQuery() : IRequest<List<UrunDto>>;

	public class GetUrunQueryHandler : IRequestHandler<GetUrunsQuery, List<UrunDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetUrunQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<UrunDto>> Handle(GetUrunsQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			

			var uruns = await _webDbContext.Urunler
				.AsNoTracking()
				.OrderByDescending(order => order.CreatedAt)
				.Select(uruns => uruns.MapToUrunDto())
				.ToListAsync(cancellationToken);

			return uruns;
		}
	}
}
