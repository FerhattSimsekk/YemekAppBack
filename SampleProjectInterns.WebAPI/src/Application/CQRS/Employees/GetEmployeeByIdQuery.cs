using Application.Dtos.Employees.Response;      // Çalışan yanıt DTO'su için gerekli namespace
using Application.Interfaces;                   // Uygulama arayüzleri için gerekli namespace
using Application.Mappers;                      // Veri nesnelerini DTO'lara dönüştürmek için gerekli namespace
using Domain.Exceptions;                        // Özel istisnalar için gerekli namespace
using MediatR;                                  // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;            // Entity Framework Core ile ilgili namespace'ler
using System.Security.Principal;                // Güvenlik ile ilgili namespace'ler

namespace Application.CQRS.Employees
{
    // GetEmployeeByIdQuery, belirli bir çalışanın detaylarını getirmek için kullanılan bir isteği temsil eder.
    public record GetEmployeeByIdQuery(long Id) : IRequest<EmployeeDto>;

    // GetEmployeeByIdQueryHandler, GetEmployeeByIdQuery isteğini işleyen bir sınıftır.
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        // GetEmployeeByIdQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetEmployeeByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        // Handle metodu, GetEmployeeByIdQuery isteğini işler ve belirli bir çalışanın DTO'sunu döndürür.
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği alınır, eğer bulunamazsa hata fırlatılır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Çalışanın detayları veritabanından alınır, eğer bulunamazsa hata fırlatılır.
            var employee = await _webDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException($"Employee not found", "Customer");

            // Çalışan DTO'ya dönüştürülerek döndürülür.
            return employee.MapToEmployeeDto();
        }
    }
}
