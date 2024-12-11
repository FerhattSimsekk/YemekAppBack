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

		public CreateUrunCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
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
				ResimUrl = request.Urun.resimUrl,
				Kategori = request.Urun.kategori,
				Status = Status.approved,

			};
			await _webDbContext.Urunler.AddAsync(urun, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return urun.MapToUrunDto();
		}
	}
}
