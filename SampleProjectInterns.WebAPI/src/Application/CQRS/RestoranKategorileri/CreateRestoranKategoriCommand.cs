using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using Application.Mappers;

using static SampleProjectInterns.Entities.Common.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Application.Dtos.RestoranKategorileri.Request;
using Application.Dtos.RestoranKategorileri.Response;


namespace Application.CQRS.RestoranKategorileri
{
	public record CreateRestoranKategoriCommand(RestoranKategoriCreateDTO RestoranKategori) : IRequest<RestoranKategoriDTO>;
	public class CreateRestoranKategoriCommandHandler : IRequestHandler<CreateRestoranKategoriCommand, RestoranKategoriDTO>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public CreateRestoranKategoriCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<RestoranKategoriDTO> Handle(CreateRestoranKategoriCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Restoran");

			KategoriRestoran kategoriRestoranim = new()
			{
				Ad = request.RestoranKategori.ad,

			};
			

			if (request.RestoranKategori.resimUrl is not null)
			{
				await _storage.Put($"{kategoriRestoranim.Id}/{request.RestoranKategori.resimUrl.FileName.Split('.')[0]}.", request.RestoranKategori?.resimUrl?.OpenReadStream(), request.RestoranKategori.resimUrl.FileName.Split('.').Last().ToString(), cancellationToken);
				kategoriRestoranim.ResimUrl = $"Shared/{kategoriRestoranim.Id}/{request.RestoranKategori.resimUrl.FileName}";
				await _webDbContext.SaveChangesAsync(cancellationToken);
			}
			await _webDbContext.KategoriRestoranlar.AddAsync(kategoriRestoranim, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return kategoriRestoranim.MaptoRestoranKategoriDto();
		}
	}
}
