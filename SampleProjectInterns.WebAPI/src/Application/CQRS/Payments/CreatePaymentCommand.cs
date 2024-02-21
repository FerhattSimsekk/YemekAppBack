using Application.Dtos.Payments.Request;
using Application.Dtos.Payments.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Application.CQRS.Payments;

public  record CreatePaymentCommand(PaymentCreateDto payment):IRequest<PaymentDto>
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        public CreatePaymentHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }


        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var identity = await _webDbContext.Identities.AsNoTracking().
            FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)??
            
            throw new Exception("User Not Found");


            var payment = new Payment
            {
                CompanyId=request.payment.company_id,
                CustomerId = request.payment.customer_id,
                Price= request.payment.price,   
                BillNumber= request.payment.bill_number,
                Description= request.payment.description,
                LastPaymentDate=request.payment.lat_payment_date,
                Status = SampleProjectInterns.Entities.Common.Enums.Status.approved,



            };
            await _webDbContext.Payments.AddAsync(payment,cancellationToken);
            await _webDbContext.SaveChangesAsync(cancellationToken);
            return payment.MapToPaymentDto();

        }

    }

}
