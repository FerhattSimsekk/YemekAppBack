using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Restorans.Request;
using Application.Dtos.Restorans.Response;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using Application.Mappers;

using static SampleProjectInterns.Entities.Common.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Application.CQRS.Restorans
{
	public record CreateRestoranCommand(RestoranCreateDto Restoran) : IRequest<RestoranDto>;
	public class CreateRestoranCommandHandler : IRequestHandler<CreateRestoranCommand, RestoranDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateRestoranCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<RestoranDto> Handle(CreateRestoranCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Restoran");

			Restoran restoran = new()
			{
				Ad=request.Restoran.ad,
				Telefon=request.Restoran.telefon,
				Adres=request.Restoran.adres,
				Hakkinda=request.Restoran.hakkinda,
				AcikMi=request.Restoran.acikMi,
				CalismaSaatleri=request.Restoran.calismaSaatleri,
				TahminiTeslimatSüresi= request.Restoran.tahminiTeslimatSüresi,
				Status= Status.approved,
				KategoriRestoranId=request.Restoran.KategoriRestoranId,

			};
			var kategoriAd = await _webDbContext.KategoriRestoranlar
			.Where(k => k.Id == request.Restoran.KategoriRestoranId)
			.Select(k => k.Ad)
			.FirstOrDefaultAsync(cancellationToken)
			?? "Unknown"; // Kategori bulunamazsa "Unknown" döndürüyoruz

			if (request.Restoran.resimUrl is not null)
			{
				await _storage.Put($"{restoran.Id}/{request.Restoran.resimUrl.FileName.Split('.')[0]}.", request.Restoran?.resimUrl?.OpenReadStream(), request.Restoran.resimUrl.FileName.Split('.').Last().ToString(), cancellationToken);
				restoran.ResimUrl = $"Shared/{restoran.Id}/{request.Restoran.resimUrl.FileName}";
				await _webDbContext.SaveChangesAsync(cancellationToken);
			}
			await _webDbContext.Restoranlar.AddAsync(restoran, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return restoran.MaptoRestoranDto(kategoriAd);
		}
	}
}
