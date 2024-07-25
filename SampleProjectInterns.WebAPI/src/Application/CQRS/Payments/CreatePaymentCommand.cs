using Application.Dtos.Customers.Request;
using Application.Dtos.Customers.Response;
using Application.Interfaces.Mailing;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Payments.Request;
using Application.Dtos.Payments.Response;
using Microsoft.EntityFrameworkCore;
using Application.Mappers;

namespace Application.CQRS.Payments
{
	public record CreatePaymentCommand(PaymentCreateDto Payment) : IRequest<PaymentDto>;

	public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		public CreatePaymentCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}


		public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");


			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Payment");

			Payment payment = new()
			{
				BillNumber = request.Payment.bill_number,
				CustomerId = request.Payment.customer_id,
				Description = request.Payment.description,
				Price = request.Payment.price,
				LastPaymentDate = request.Payment.last_payment_date,
				CompanyId = request.Payment.company_id,
				Status = SampleProjectInterns.Entities.Common.Enums.Status.approved

			};
			await _webDbContext.Payments.AddAsync(payment, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return payment.MapToPaymentDto();
		}
	}
}
