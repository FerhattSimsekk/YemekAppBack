using Application.Dtos.Identities.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identities
{
    // GetIdentityQuery: Kimlik bilgisini sorgulamak için kullanılan bir sorgu nesnesi
    public record GetIdentityQuery() : IRequest<IdentityDto>;

    // GetIdentityQueryHandler: GetIdentityQuery sorgusunu işleyen sınıf
    public class GetIdentityQueryHandler : IRequestHandler<GetIdentityQuery, IdentityDto?>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı
        private readonly IPrincipal _principal; // Kullanıcı kimlik bilgisi

        // Constructor: Bağımlılıkları enjekte eder
        public GetIdentityQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle Metodu: GetIdentityQuery sorgusunu işler
        public async Task<IdentityDto?> Handle(GetIdentityQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimlik bilgisini e-posta adresine ve onaylanmış durumuna göre veritabanından çeker
            var identity = await _webDbContext.Identities.AsNoTracking()
              .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name && identity.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved, cancellationToken)
              ?? throw new Exception("User not found");

            // Eğer kimlik bilgisi bulunamazsa null döner
            if (identity == null) return null;

            // Kimlik bilgisini IdentityDto'ya dönüştürür ve döndürür
            return identity.MapToIdentityDto();
        }
    }
}
