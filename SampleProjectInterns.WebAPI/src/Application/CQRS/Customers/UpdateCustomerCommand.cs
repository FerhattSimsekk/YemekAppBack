using Application.Dtos.Customers.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Customers
{
    // UpdateCustomerCommand, bir müşterinin bilgilerini güncellemek için kullanılan bir isteği temsil eder.
    public record UpdateCustomerCommand(CustomerUpdateDto Customer, long CustomerId) : IRequest;

    // UpdateCustomerCommandHandler, UpdateCustomerCommand isteğini işleyen bir sınıftır.
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // UpdateCustomerCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public UpdateCustomerCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, UpdateCustomerCommand isteğini işler.
        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Güncellenecek müşteri veritabanından alınır, bulunamazsa hata alınır.
            var customer = await _webDbContext.Customers.FirstOrDefaultAsync(i => i.Id == request.CustomerId, cancellationToken)
                ?? throw new NotFoundException($"{request.Customer.name} not found", "Customer");

            // Müşteri bilgileri güncellenir.
            customer.Address = request.Customer.address;
            customer.Description = request.Customer.description;
            customer.Mail = request.Customer.mail;
            customer.Name = request.Customer.name;
            customer.Surname = request.Customer.surname;
            customer.Gender = request.Customer.gender;
            customer.Phone = request.Customer.phone;
            customer.Status = request.Customer.status;

            // Değişiklikler kaydedilir.
            await _webDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
