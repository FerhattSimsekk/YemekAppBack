using Application.Dtos.Identities.Request;
using Application.Dtos.Identities.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Identities
{
    // IdentityAuthenticationQuery: Kimlik doğrulama sorgusunu temsil eden sınıf
    public record IdentityAuthenticationQuery(IdentityVerifyDto Identity) : IRequest<IdentityDto>;

    // IdentityAuthenticationQueryHandler: IdentityAuthenticationQuery sorgusunu işleyen sınıf
    public class IdentityAuthenticationQueryHandler : IRequestHandler<IdentityAuthenticationQuery, IdentityDto?>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı

        // Constructor: Bağımlılıkları enjekte eder
        public IdentityAuthenticationQueryHandler(IWebDbContext webDbContext)
        {
            _webDbContext = webDbContext;
        }

        // Handle Metodu: IdentityAuthenticationQuery sorgusunu işler
        public async Task<IdentityDto?> Handle(IdentityAuthenticationQuery request, CancellationToken cancellationToken)
        {
            // Kimlik doğrulama için kullanıcıyı e-posta adresi ve onaylanmış durumuna göre veritabanından çeker
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == request.Identity.Email && identity.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved, cancellationToken);

            // Eğer kimlik bilgisi bulunamazsa null döner
            if (identity == null) return null;

            // Kullanıcının şifresini, veritabanındaki şifre ile karşılaştırarak doğrulama yapar
            var isPasswordVerified = BCrypt.Net.BCrypt.HashPassword(request.Identity.Password, identity.Salt)
                == identity.Password;

            // Şifre doğrulanırsa kimlik bilgisini IdentityDto'ya dönüştürür ve döndürür, doğrulanamazsa null döner
            return isPasswordVerified ? identity.MapToIdentityDto() : null;
        }
    }
}
