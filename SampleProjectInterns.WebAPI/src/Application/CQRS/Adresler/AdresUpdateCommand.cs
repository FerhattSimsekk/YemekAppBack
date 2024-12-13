using Application.Dtos.Adresler.Request;
using Application.Dtos.Adresler.Response;
using Application.Interfaces;
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
	public record UpdateAdresCommand(AdresUpdateDto adres, long adresId) :   IRequest;
	public class UpdateAdresCommandHandler : IRequestHandler<UpdateAdresCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public UpdateAdresCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<Unit> Handle(UpdateAdresCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");



			var adres1 = await _webDbContext.Adresler.FirstOrDefaultAsync(i => i.Id == request.adresId, cancellationToken)
				?? throw new NotFoundException($"{request.adres.Baslik} not found", "adres");



			adres1.Baslik = request.adres.Baslik;
			adres1.AdresBilgisi = request.adres.AdresBilgisi;
			adres1.CityId = request.adres.CityId;
			adres1.CountyId = request.adres.CountyId;
			adres1.IdentityId = identity.Id;
			adres1.Status = Status.approved;




			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
