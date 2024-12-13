using Application.Dtos.Adresler.Request;
using Application.Dtos.Adresler.Response;
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

namespace Application.CQRS.Adresler
{
	public record CreateAdresCommand(AdresCreateDto adres) : IRequest<AdresDto>;
	public class CreateAdresCommandHandler : IRequestHandler<CreateAdresCommand, AdresDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateAdresCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<AdresDto> Handle(CreateAdresCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			

			Adres adres = new()
			{
							

				Baslik = request.adres.Baslik,
				AdresBilgisi = request.adres.AdresBilgisi,
				CityId = request.adres.CityId,
				CountyId = request.adres.CountyId,
				IdentityId = identity.Id,
				Status = Status.approved,

			};


			
			await _webDbContext.Adresler.AddAsync(adres, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return adres.MapToAdresDto();
		}
	}
}
