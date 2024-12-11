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

namespace Application.CQRS.Restorans
{
	public record DeleteRestoranCommand(long Id) : IRequest;

	public class DeleteRestoranCommandHandler : IRequestHandler<DeleteRestoranCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public DeleteRestoranCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(DeleteRestoranCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Restoran");



			var restoran = await _webDbContext.Restoranlar.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
				?? throw new NotFoundException($"Restoran Not found", "Restoran");


			restoran.Status = Status.deleted;
			await _webDbContext.SaveChangesAsync(cancellationToken);


			return Unit.Value;
		}
	}
}
