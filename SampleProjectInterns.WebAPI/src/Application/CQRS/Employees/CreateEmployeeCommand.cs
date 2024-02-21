using Application.Dtos.Employees.Request;           // Çalışan oluşturma isteği için gerekli DTO namespace'i
using Application.Dtos.Employees.Response;          // Çalışan yanıt DTO'ları için gerekli DTO namespace'i
using Application.Interfaces;                       // Uygulama arayüzleri için gerekli namespace
using Application.Mappers;                          // Veri nesnelerini DTO'lara dönüştürmek için gerekli namespace
using MediatR;                                      // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;                // Entity Framework Core ile ilgili namespace'ler
using SampleProjectInterns.Entities;                // Proje özgü varlık namespace'i
using System.Security.Principal;                    // Güvenlik ile ilgili namespace'ler

namespace Application.CQRS.Employees
{
    // CreateEmployeeCommand, yeni bir çalışan oluşturma isteğini temsil eder.
    public record CreateEmployeeCommand(EmployeeCreateDto Employee) : IRequest<EmployeeDto>;

    // CreateEmployeeHandler, CreateEmployeeCommand isteğini işleyen bir sınıftır.
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        // CreateEmployeeHandler, gerekli bağımlılıkları alarak oluşturulur.
        public CreateEmployeeHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        // Handle metodu, CreateEmployeeCommand isteğini işler ve yeni çalışanın DTO'sunu döndürür.
        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği alınır, eğer bulunamazsa hata fırlatılır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User Not Found");

            // Yeni çalışan oluşturulur ve veriler atanır.
            var employee = new Employee
            {
                Name = request.Employee.name,
                Surname = request.Employee.surname,
                Gender = request.Employee.gender,
                Phone = request.Employee.phone,
                Phone2 = request.Employee.phone2,
                Mail = request.Employee.mail,
                Description = request.Employee.description,
                Department = request.Employee.department,
                Address = request.Employee.address,
                Status = SampleProjectInterns.Entities.Common.Enums.Status.approved,
                CompanyId = identity.CompanyId // Şirket kimliği atanır
            };

            // Oluşturulan çalışan veritabanına eklenir.
            await _webDbContext.Employees.AddAsync(employee, cancellationToken);
            await _webDbContext.SaveChangesAsync(cancellationToken);

            // Oluşturulan çalışan DTO'ya dönüştürülerek döndürülür.
            return employee.MapToEmployeeDto();
        }
    }
}
