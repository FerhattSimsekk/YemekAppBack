using Application.Dtos.Uruns.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;




	namespace Application.CQRS.Uruns
	{
		public record GetUrunByIdQuery(long Id) : IRequest<UrunDto>;

		public class GetUrunByIdQueryHandler : IRequestHandler<GetUrunByIdQuery, UrunDto>
		{
			private readonly IWebDbContext _webDbContext;
			private readonly IPrincipal _principal;

			public GetUrunByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
			{
				_webDbContext = webDbContext;
				_principal = principal;
			}

			public async Task<UrunDto> Handle(GetUrunByIdQuery request, CancellationToken cancellationToken)
			{
				var identity = await _webDbContext.Identities.AsNoTracking()
				 .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				 ?? throw new Exception("User not found");
				




				var urun = await _webDbContext.Urunler.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
				   ?? throw new NotFoundException($"Urun not found", "Urun");



				return urun.MapToUrunDto();
			}
		}
	}


