using Application.Dtos.Uruns.Request;
using Application.Interfaces;
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
	public record UpdateUrunCommand(UrunUpdateDto Urun, long UrunId) : IRequest;
	public class UpdateUrunCommandHandler : IRequestHandler<UpdateUrunCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public UpdateUrunCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<Unit> Handle(UpdateUrunCommand request, CancellationToken cancellationToken)
		{

			var identity = await _webDbContext.Identities.AsNoTracking()
			.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			?? throw new Exception("User not found");
			var auth = identity.Type;
			if (auth is not AdminAuthorization.admin && auth is not AdminAuthorization.moderator)
				throw new UnAuthorizedException("Unauthorized access", "Urun");

			var urun = await _webDbContext.Urunler.FirstOrDefaultAsync(i => i.Id == request.UrunId, cancellationToken)
				?? throw new NotFoundException($"{request.Urun.ad} not found", "Urun");

			urun.RestoranId = request.Urun.restoranId;
			urun.Ad = request.Urun.ad;
			urun.Fiyat = request.Urun.fiyat;
			urun.Aciklama = request.Urun.aciklama;
			urun.Kategori = request.Urun.kategori;
			urun.Aktif = request.Urun.aktif;
			urun.Status = Status.approved;

			if (request.Urun.resimUrl is not null)
			{
				await _storage.Put($"{urun.Id}/{request.Urun.resimUrl.FileName.Split('.')[0]}.", request.Urun?.resimUrl?.OpenReadStream(), request.Urun.resimUrl.FileName.Split('.').Last().ToString(), cancellationToken);
				urun.ResimUrl = $"Shared/{urun.Id}/{request.Urun.resimUrl.FileName}";
				await _webDbContext.SaveChangesAsync(cancellationToken);
			}

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

		}
	}
}
