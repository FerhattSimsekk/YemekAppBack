
using Application.Dtos.Payments.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Payments
{

    public record UpdatePaymentCommand(PaymentUpdateDto Payment, long PaymentsId) : IRequest;


    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        public UpdatePaymentCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {

            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");


            var payment = await _webDbContext.Payments.FirstOrDefaultAsync(i => i.Id == request.PaymentsId, cancellationToken)
                ?? throw new NotFoundException($"{request.Payment.bill_number} not found", "Payment");


            payment.CompanyId = request.Payment.company_id;
            payment.CustomerId = request.Payment.customer_id;
            payment.Price = request.Payment.price;
            payment.BillNumber = request.Payment.bill_number;
            payment.Description = request.Payment.description;
            payment.LastPaymentDate = request.Payment.lat_payment_date;
            payment.Status = request.Payment.status;


            await _webDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }




}


