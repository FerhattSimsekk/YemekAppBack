using Application.Dtos.Customers.Request;
using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;

namespace Application.CQRS.Customers
{
    // CreateCustomerCommand, bir müşteri oluşturmak için kullanılan bir isteği temsil eder.
    public record CreateCustomerCommand(CustomerCreateDto Customer) : IRequest<CustomerDto>;

    // CreateCustomerHandler, CreateCustomerCommand isteğini işleyen bir sınıftır.
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // CreateCustomerHandler, gerekli bağımlılıkları alarak oluşturulur.
        public CreateCustomerHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, CreateCustomerCommand isteğini işler ve CustomerDto döndürür.
        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
              .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
              ?? throw new Exception("User not found");

            // Yeni müşteri oluşturulur.
            Customer customer = new()
            {
                Address = request.Customer.address,
                CompanyId = request.Customer.company_id,
                Description = request.Customer.description,
                Gender = request.Customer.gender,
                Mail = request.Customer.mail,
                Phone = request.Customer.phone,
                Status = SampleProjectInterns.Entities.Common.Enums.Status.approved,
                Name = request.Customer.name,
                Surname = request.Customer.surname
            };

            // Müşteri veritabanına eklenir.
            await _webDbContext.Customers.AddAsync(customer, cancellationToken);
            await _webDbContext.SaveChangesAsync(cancellationToken);

            // Oluşturulan müşteri CustomerDto'ya dönüştürülerek döndürülür.
            return customer.MapToCustomerDto();
        }
    }
}
