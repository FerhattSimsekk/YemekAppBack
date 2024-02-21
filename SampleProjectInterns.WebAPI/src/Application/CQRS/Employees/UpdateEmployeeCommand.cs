using Application.Dtos.Employees.Request;       // Çalışan istek DTO'su için gerekli namespace
using Application.Interfaces;                   // Uygulama arayüzleri için gerekli namespace
using Domain.Exceptions;                        // Özel istisna sınıfları için gerekli namespace
using MediatR;                                  // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;            // Entity Framework Core ile ilgili namespace'ler
using System.Security.Principal;                // Güvenlik ile ilgili namespace'ler

namespace Application.CQRS.Employees
{
    // UpdateEmployeeCommand, çalışan bilgilerini güncellemek için kullanılan bir komutu temsil eder.
    public record UpdateEmployeeCommand(EmployeeUpdateDto Employee, long EmployeesId) : IRequest;

    // UpdateEmployeCommandHandler, UpdateEmployeeCommand komutunu işleyen bir sınıftır.
    public class UpdateEmployeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        // UpdateEmployeCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public UpdateEmployeCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        // Handle metodu, UpdateEmployeeCommand komutunu işler ve çalışan bilgilerini günceller.
        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği alınır, eğer bulunamazsa hata fırlatılır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Güncellenecek çalışan bilgileri veritabanından alınır, eğer bulunamazsa hata fırlatılır.
            var employee = await _webDbContext.Employees.FirstOrDefaultAsync(i => i.Id == request.EmployeesId, cancellationToken)
                ?? throw new NotFoundException($"{request.Employee.name} not found", "Employee");

            // Çalışan bilgileri güncellenir.
            employee.Name = request.Employee.name;
            employee.Surname = request.Employee.surname;
            employee.Gender = request.Employee.gender;
            employee.Mail = request.Employee.mail;
            employee.Phone = request.Employee.phone;
            employee.Phone2 = request.Employee.phone2;
            employee.Address = request.Employee.address;
            employee.Department = request.Employee.department;
            employee.Description = request.Employee.description;
            employee.Status = request.Employee.status;

            // Değişiklikler veritabanına kaydedilir.
            await _webDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value; // Başarılı tamamlanma işareti döndürülür.
        }
    }
}
