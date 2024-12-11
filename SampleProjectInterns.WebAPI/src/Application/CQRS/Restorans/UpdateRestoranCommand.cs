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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Application.CQRS.Restorans
{
	public record UpdateRestoranCommand(RestoranUpdateDto Restoran, long RestoranId) : IRequest;
	public class UpdateRestoranCommandHandler : IRequestHandler<UpdateRestoranCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public UpdateRestoranCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<Unit> Handle(UpdateRestoranCommand request, CancellationToken cancellationToken)
		{

			var identity = await _webDbContext.Identities.AsNoTracking()
			.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Restoran");

			var restoran = await _webDbContext.Restoranlar.FirstOrDefaultAsync(i => i.Id == request.RestoranId, cancellationToken)
				?? throw new NotFoundException($"{request.Restoran.ad} not found", "Restoran");


			restoran.Ad = request.Restoran.ad;
			restoran.Telefon = request.Restoran.telefon;
			restoran.Adres = request.Restoran.adres;
			restoran.Hakkinda = request.Restoran.hakkinda;
			restoran.AcikMi = request.Restoran.acikMi;
			restoran.CalismaSaatleri = request.Restoran.calismaSaatleri;
			restoran.TahminiTeslimatSüresi = request.Restoran.tahminiTeslimatSüresi;
			restoran.Status = Status.approved;
			restoran.UpdatedAt= DateTime.Now;
			restoran.KategoriRestoranId=request.Restoran.KategoriRestoranId;
			if (request.Restoran.resimUrl is not null)
			{
				restoran.ResimUrl = $"Shared/{restoran.Id}/{request.Restoran.resimUrl.FileName}";
				await _storage.Put($"{restoran.Id}/{request.Restoran.resimUrl.FileName.Split('.')[0]}.", request.Restoran?.resimUrl?.OpenReadStream(), request.Restoran.resimUrl.FileName.Split('.').Last().ToString(), cancellationToken);
			}

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

		}
	}
}
