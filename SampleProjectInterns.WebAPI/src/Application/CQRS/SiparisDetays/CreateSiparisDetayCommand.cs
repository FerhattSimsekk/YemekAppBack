using Application.Dtos.Restorans.Request;
using Application.Dtos.Restorans.Response;
using Application.Dtos.SiparisDetays;
using Application.Dtos.SiparisDetays.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.SiparisDetays
{
	public record CreateSiparisDetayCommand(List<SiparisDetayCreateDto> SiparisDetay,long RestoranId,long AdresId) : IRequest<bool>;
	public class CreateSiparisDetayCommandHandler : IRequestHandler<CreateSiparisDetayCommand, bool>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public CreateSiparisDetayCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<bool> Handle(CreateSiparisDetayCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			Siparis siparis = new()
			{
				AdresId=request.AdresId,
				RestoranId = request.RestoranId,
				Durum = SiparisDurumu.Hazirlaniyor,
				IdentityId = identity.Id,
				ToplamTutar = request.SiparisDetay.Sum(x => x.Adet * x.Fiyat),
				Status = Status.approved,
				OlusturmaTarihi=DateTime.Now.ToUniversalTime(),
				TeslimTarihi=DateTime.Now.AddHours(1).ToUniversalTime(),
				
			};
			await _webDbContext.Siparisler.AddAsync(siparis, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);



			if(request.SiparisDetay is { Count:>0})
				foreach (var item in request.SiparisDetay)
				{
					SiparisDetay siparisDetay = new()
					{
						SiparisId=siparis.Id,
						UrunId=item.UrunId,
						Adet = item.Adet,
						Fiyat = item.Fiyat,
						Status = Status.approved,
						Toplam=item.Adet*item.Fiyat

					};
					await _webDbContext.SiparisDetaylar.AddAsync(siparisDetay, cancellationToken);
					await _webDbContext.SaveChangesAsync(cancellationToken);
				}








			return true;
		}
	}
}
