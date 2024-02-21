using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Customers
{
    // DeleteCustomerCommand, bir müşteriyi silmek için kullanılan bir isteği temsil eder.
    public record DeleteCustomerCommand(long Id) : IRequest;

    // DeleteCustomerCommandHandler, DeleteCustomerCommand isteğini işleyen bir sınıftır.
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // DeleteCustomerCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public DeleteCustomerCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, DeleteCustomerCommand isteğini işler.
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User Not Found");

            // Silinecek müşteri veritabanından alınır, bulunamazsa hata alınır.
            var customer = await _webDbContext.Customers.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Customer Not found", "Customer");

            // Müşteri durumu "deleted" olarak güncellenir ve değişiklikler kaydedilir.
            customer.Status = Status.deleted;
            await _webDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
