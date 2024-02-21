using Application.Interfaces;               // Uygulama arayüzleri için gerekli namespace
using Domain.Exceptions;                    // Özel istisnalar için gerekli namespace
using MediatR;                              // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;        // Entity Framework Core ile ilgili namespace'ler
using System.Security.Principal;            // Güvenlik ile ilgili namespace'ler
using static SampleProjectInterns.Entities.Common.Enums; // Enums namespace'i

namespace Application.CQRS.Employees
{
    // DeleteEmployeeCommand, belirli bir çalışanın silinmesi için kullanılan bir isteği temsil eder.
    public record DeleteEmployeeCommand(long Id) : IRequest;

    // DeleteEmployeeCommandHandler, DeleteEmployeeCommand isteğini işleyen bir sınıftır.
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı
        private readonly IPrincipal _principal; // Kullanıcı kimliği referansı

        // DeleteEmployeeCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public DeleteEmployeeCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
            _principal = principal; // Kullanıcı kimliği enjekte edilir
        }

        // Handle metodu, DeleteEmployeeCommand isteğini işler ve çalışanın durumunu silinmiş olarak günceller.
        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı kimliği alınır, eğer bulunamazsa hata fırlatılır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User Not Found");

            // Silinecek çalışan bulunur, eğer bulunamazsa hata fırlatılır.
            var employee = await _webDbContext.Employees.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Employee Not found", "Employee");

            // Çalışanın durumu "silinmiş" olarak güncellenir.
            employee.Status = Status.deleted;
            await _webDbContext.SaveChangesAsync(cancellationToken);

            // İşlem başarılı bir şekilde tamamlandığında Unit.Value döndürülür.
            return Unit.Value;
        }
    }
}
