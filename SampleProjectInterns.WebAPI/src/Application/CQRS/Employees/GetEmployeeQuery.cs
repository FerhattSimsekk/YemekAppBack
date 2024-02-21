using Application.Dtos.Employees.Response;      // Çalışan yanıt DTO'su için gerekli namespace
using Application.Interfaces;                   // Uygulama arayüzleri için gerekli namespace
using Application.Mappers;                      // Veri nesnelerini DTO'lara dönüştürmek için gerekli namespace
using MediatR;                                  // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;            // Entity Framework Core ile ilgili namespace'ler
using System.Security.Principal;                // Güvenlik ile ilgili namespace'ler

namespace Application.CQRS.Employees
{
    // GetEmployeeQuery, tüm çalışanları getirmek için kullanılan bir isteği temsil eder.
    public record GetEmployeeQuery() : IRequest<List<EmployeeDto>>;

    // GetEmployeeHandler, GetEmployeeQuery isteğini işleyen bir sınıftır.
    public class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, List<EmployeeDto>>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        // GetEmployeeHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetEmployeeHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        // Handle metodu, GetEmployeeQuery isteğini işler ve tüm çalışanların DTO listesini döndürür.
        public async Task<List<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği alınır, eğer bulunamazsa hata fırlatılır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Tüm çalışanlar veritabanından alınır, DTO'ya dönüştürülerek döndürülür.
            var employees = await _webDbContext.Employees.AsNoTracking()
                .OrderByDescending(order => order.CreatedAt)
                .Select(employee => employee.MapToEmployeeDto())
                .ToListAsync(cancellationToken);

            return employees;
        }
    }
}
