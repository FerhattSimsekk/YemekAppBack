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

namespace Application.CQRS.Payments
{
    public record DeletePaymentCommand(long id):IRequest;
    public class DeletePaymentCommandHandler:IRequestHandler<DeletePaymentCommand>
    {

        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        public DeletePaymentCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var identity=await _webDbContext.Identities.AsNoTracking().
                FirstOrDefaultAsync(identity=>identity.Email==_principal.Identity!.Name,cancellationToken)??
            throw new Exception("User Not Found");
            var payment = await _webDbContext.Payments.FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken) ??
                throw new NotFoundException($"payment not found ", "Payment");
            payment.Status=Status.deleted;
            await _webDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
