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
using Application.Dtos.Yorumlar.Request;
using Application.Dtos.Yorumlar.Response;

namespace Application.CQRS.Yorumlar
{
	public record CreateYorumCommand(YorumCreateDto yorum) : IRequest<bool>;
	public class CreateYorumCommandHandler : IRequestHandler<CreateYorumCommand, bool>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;



		public CreateYorumCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<bool> Handle(CreateYorumCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");


			var siparis = await _webDbContext.Siparisler
				.FirstOrDefaultAsync(s => s.Id == request.yorum.SiparisId, cancellationToken)
				?? throw new Exception("Order Not Found");
			if (siparis.yorumYapildiMi)
			{
				throw new Exception("A review has already been made for this order.");
			}

			if (siparis.Durum != SiparisDurumu.TeslimEdildi)
			{
				throw new Exception("You can only leave a review for delivered orders.");
			}

			Yorum yorum = new()
			{


				Derecelendirme = request.yorum.Derecelendirme,
				YorumMetni = request.yorum.YorumMetni,
				SiparisId = request.yorum.SiparisId,
				IdentityId = identity.Id,
				Status = Status.approved,

			};



			await _webDbContext.Yorumlar.AddAsync(yorum, cancellationToken);
			siparis.yorumYapildiMi = true;
			await _webDbContext.SaveChangesAsync(cancellationToken);







			return true;
		}
	}
}
