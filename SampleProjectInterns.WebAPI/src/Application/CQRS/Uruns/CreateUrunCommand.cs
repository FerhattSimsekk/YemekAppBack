using Application.Dtos.Restorans.Request;
using Application.Dtos.Restorans.Response;
using Application.Dtos.Uruns.Request;
using Application.Dtos.Uruns.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Uruns
{
	public record CreateUrunCommand(UrunCreateDto Urun) : IRequest<UrunDto>;
	public class CreateUrunCommandHandler : IRequestHandler<CreateUrunCommand, UrunDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateUrunCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
			}

		public async Task<UrunDto> Handle(CreateUrunCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			var auth = identity.Type;
			if (auth is not AdminAuthorization.admin && auth is not AdminAuthorization.moderator)
				throw new UnAuthorizedException("Unauthorized access", "Urun");

			Urun urun = new()
			{
				RestoranId=request.Urun.restoranId,
				Ad = request.Urun.ad,
				Fiyat = request.Urun.fiyat,
				Aciklama = request.Urun.aciklama,
				Kategori = request.Urun.kategori,
				Status = Status.approved,

			};


			if (request.Urun.resimUrl is not null)
			{
				await _storage.Put($"{urun.Id}/{request.Urun.resimUrl.FileName.Split('.')[0]}.", request.Urun?.resimUrl?.OpenReadStream(), request.Urun.resimUrl.FileName.Split('.').Last().ToString(), cancellationToken);
				urun.ResimUrl = $"Shared/{urun.Id}/{request.Urun.resimUrl.FileName}";
				await _webDbContext.SaveChangesAsync(cancellationToken);
			}
			await _webDbContext.Urunler.AddAsync(urun, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return urun.MapToUrunDto();
		}
	}
}
