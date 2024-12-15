using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;
using Application.Dtos.Restorans.Request;
using Application.Dtos.Restorans.Response;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using Application.Mappers;


namespace Application.CQRS.Restorans
{
	public record GetRestoransByKategoriIdQuery(long id) : IRequest<List<RestoranDto>>;

	public class GetRestoransByKategoriIdQueryHandler : IRequestHandler<GetRestoransByKategoriIdQuery, List<RestoranDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetRestoransByKategoriIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<RestoranDto>> Handle(GetRestoransByKategoriIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");



			//var restorans = await _webDbContext.Restoranlar
			//	.AsNoTracking()
			//	.OrderByDescending(order => order.CreatedAt)
			//	.Select(restorans => restorans.MaptoRestoranDto())
			//	.ToListAsync(cancellationToken);
			var restorans = await _webDbContext.Restoranlar
	.AsNoTracking()
	.Where(restoran => restoran.KategoriRestoranId == request.id)
	.OrderByDescending(order => order.CreatedAt)
	.Select(restoran => new
	{
		Restoran = restoran,
		KategoriAdi = _webDbContext.KategoriRestoranlar
			.Where(k => k.Id == restoran.KategoriRestoranId)
			.Select(k => k.Ad)
			.FirstOrDefault()
	})
	
	.ToListAsync(cancellationToken);

			var restoransDto = restorans
			.Select(r => r.Restoran.MaptoRestoranDto(r.KategoriAdi ?? "Kategori Yok"))
			.ToList();

			return restoransDto;
		}
	}
}
