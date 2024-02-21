using Application.Dtos.Employees.Response;
using Application.Dtos.Payments.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Payments
{
    public record GetPaymentQuery() : IRequest<List<PaymentDto>>;
    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, List<PaymentDto>>
    {
        private readonly IWebDbContext _webdbcontext;
        private readonly IPrincipal _principal;

        public GetPaymentHandler(IWebDbContext webdbcontext, IPrincipal principal)
        {
            _webdbcontext = webdbcontext;
            _principal = principal;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var identity = await _webdbcontext.Identities.AsNoTracking()
            .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
            ?? throw new Exception("User not found");

            var payments = await _webdbcontext.Payments.AsNoTracking().
                OrderByDescending(x => x.CreatedAt).Select(payments => payments.MapToPaymentDto()).
                ToListAsync(cancellationToken);
            return payments;
        }
    }

}
