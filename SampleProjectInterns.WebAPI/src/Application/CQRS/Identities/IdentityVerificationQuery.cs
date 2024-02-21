using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Identities
{
    // IdentityVerificationQuery: Kullanıcı kimliğinin doğrulama sorgusunu temsil eden sınıf
    public record IdentityVerificationQuery(string UserName) : IRequest<bool>;

    // IdentityVerificationQueryHandler: IdentityVerificationQuery sorgusunu işleyen sınıf
    public class IdentityVerificationQueryHandler : IRequestHandler<IdentityVerificationQuery, bool>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı

        // Constructor: Bağımlılıkları enjekte eder
        public IdentityVerificationQueryHandler(IWebDbContext webDbContext)
        {
            _webDbContext = webDbContext;
        }

        // Handle Metodu: IdentityVerificationQuery sorgusunu işler
        public async Task<bool> Handle(IdentityVerificationQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimlik bilgisini e-posta adresine ve onaylanmış durumuna göre veritabanından çeker
            var isVerified = await _webDbContext.Identities
                .AnyAsync(identity => identity.Email == request.UserName && identity.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved, cancellationToken);

            // Eğer kimlik bilgisi bulunursa true, bulunamazsa false döner
            return isVerified;
        }
    }
}
