using Application.Dtos.Payments.Request;
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

namespace Application.CQRS.Payments
{

	public record UpdatePaymentCommand(PaymentUpdateDto Payment, long PaymentId) : IRequest;
	public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public UpdatePaymentCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
			   .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			   ?? throw new Exception("User not found");
			var payment = await _webDbContext.Payments.AsNoTracking().
				FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken) ??
				throw new NotFoundException($"{request.Payment.bill_number} not found ", "Payment");

			payment.Price = request.Payment.price;
			payment.Status = request.Payment.status;
			payment.BillNumber = request.Payment.bill_number;
			payment.Description = request.Payment.description;
			payment.LastPaymentDate = request.Payment.last_payment_date;

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;


		}
	}
}
