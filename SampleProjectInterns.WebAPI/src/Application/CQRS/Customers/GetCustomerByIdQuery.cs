using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Customers
{
    // GetCustomerByIdQuery, bir müşterinin ID'sine göre bilgilerini almak için kullanılan bir isteği temsil eder.
    public record GetCustomerByIdQuery(long Id) : IRequest<CustomerDto>;

    // GetCustomerByIdQueryHandler, GetCustomerByIdQuery isteğini işleyen bir sınıftır.
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // GetCustomerByIdQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetCustomerByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, GetCustomerByIdQuery isteğini işler ve CustomerDto döndürür.
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
             .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
             ?? throw new Exception("User not found");

            // Veritabanından istenilen müşteri ID'sine sahip müşteri alınır, bulunamazsa hata alınır.
            var customer = await _webDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException($"Customer not found", "Customer");

            // Müşteri CustomerDto'ya dönüştürülerek döndürülür.
            return customer.MapToCustomerDto();
        }
    }
}
