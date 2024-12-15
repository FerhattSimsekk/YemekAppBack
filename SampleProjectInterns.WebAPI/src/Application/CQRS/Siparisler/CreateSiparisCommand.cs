//using Application.Dtos.Siparisler.Request;
//using Application.Dtos.Siparisler.Response;
//using Application.Interfaces;
//using Application.Mappers;
//using Domain.Exceptions;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using SampleProjectInterns.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;
//using static SampleProjectInterns.Entities.Common.Enums;

//namespace Application.CQRS.Siparisler
//{
//	public record CreateSiparisCommand(SiparisCreateDto Siparis) : IRequest<SiparisDto>;
//	public class CreateSiparisCommandHandler : IRequestHandler<CreateSiparisCommand, SiparisDto>
//	{
//		private readonly IWebDbContext _webDbContext;
//		private readonly IPrincipal _principal;

//		public CreateSiparisCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
//		{
//			_webDbContext = webDbContext;
//			_principal = principal;
//		}

//		public async Task<SiparisDto> Handle(CreateSiparisCommand request, CancellationToken cancellationToken)
//		{
//			var identity = await _webDbContext.Identities.AsNoTracking()
//				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
//				?? throw new Exception("User Not Found");


//			var auht = identity.Type;
//			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
//				throw new UnAuthorizedException("Unauthorized access", "Siparis");
//			var siparis = new Siparis
//			{
//				IdentityId = identity.Id,
//				TeslimTarihi = DateTime.UtcNow,
//				RestoranId = request.Siparis.RestoranId,
//				Durum=SiparisDurumu.Hazirlaniyor,
//				Status	=Status.approved,
//				SiparisDetaylari = request.Siparis.SiparisDetaylari.Select(d => new SiparisDetay
//				{
//					UrunId = d.UrunId,
//					Adet = d.Adet,
//					Fiyat = d.Fiyat
//				}).ToList()
//			};
//			await _webDbContext.Siparisler.AddAsync(siparis, cancellationToken);
//			await _webDbContext.SaveChangesAsync(cancellationToken);






//			return siparis.MapToSiparisDto();
			
			
//		}
//	}
//}
