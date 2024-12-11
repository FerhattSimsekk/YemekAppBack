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
	public record GetRestoranByIdQuery(long Id) : IRequest<RestoranDto>;

	public class GetRestoranByIdQueryHandler : IRequestHandler<GetRestoranByIdQuery, RestoranDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetRestoranByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<RestoranDto> Handle(GetRestoranByIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
			 .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			 ?? throw new Exception("User not found");





			var restoranWithCategory = await _webDbContext.Restoranlar
			.AsNoTracking()
			.Where(r => r.Id == request.Id)
			.Select(r => new
			{
				Restoran = r,
				KategoriAdi = _webDbContext.KategoriRestoranlar
					.Where(k => k.Id == r.KategoriRestoranId)
					.Select(k => k.Ad)
					.FirstOrDefault()
			})
			.FirstOrDefaultAsync(cancellationToken);

			// Eğer restoran bulunamazsa, hata fırlatıyoruz
			if (restoranWithCategory == null)
			{
				throw new NotFoundException($"Restoran with Id {request.Id} not found.", "Restoran");
			}

			// Restoranı DTO'ya dönüştürürken kategori adını da ekliyoruz
			return restoranWithCategory.Restoran.MaptoRestoranDto(restoranWithCategory.KategoriAdi ?? "Kategori Yok");
		}
	}
}
