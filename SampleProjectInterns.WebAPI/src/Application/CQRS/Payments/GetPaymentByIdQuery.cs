using Application.Dtos.Customers.Response;
using Application.Dtos.Payments.Response;
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

namespace Application.CQRS.Payments
{
	public record GetPaymentByIdQuery(long Id) : IRequest<PaymentDto>;

	public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetPaymentByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
			 .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			 ?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Payment");




			var payment = await _webDbContext.Payments.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
			   ?? throw new NotFoundException($"Customer not found", "Payment");



			return payment.MapToPaymentDto();
		}
	}
}
