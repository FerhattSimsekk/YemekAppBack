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
	public record DeleteUrunCommand(long Id) : IRequest;

	public class DeleteUrunCommandHandler : IRequestHandler<DeleteUrunCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public DeleteUrunCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(DeleteUrunCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");
			var auth = identity.Type;
			if (auth is not AdminAuthorization.admin && auth is not AdminAuthorization.moderator)
				throw new UnAuthorizedException("Unauthorized access", "Urun");



			var urun = await _webDbContext.Urunler.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
				?? throw new NotFoundException($"Urun Not found", "Urun");


			urun.Status = Status.deleted;
			await _webDbContext.SaveChangesAsync(cancellationToken);


			return Unit.Value;
		}
	}
}
