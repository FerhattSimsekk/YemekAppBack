using Application.Dtos.Counties.Response;       // İlçe yanıt DTO'larını içeren namespace
using Application.Interfaces;                   // Uygulama arayüzleri için gerekli namespace
using Application.Mappers;                      // Veri nesnelerini DTO'lara dönüştürmek için gerekli namespace
using MediatR;                                  // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;            // Entity Framework Core ile ilgili namespace'ler

namespace Application.CQRS.Definitions
{
    // GetCountiesQuery, belirli bir şehrin ilçelerinin listesini getirmek için kullanılan bir isteği temsil eder.
    public record GetCountiesQuery(int CityId) : IRequest<List<DistrictDto>>;

    // GetCountiesQueryHandler, GetCountiesQuery isteğini işleyen bir sınıftır.
    public class GetCountiesQueryHandler : IRequestHandler<GetCountiesQuery, List<DistrictDto>>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı

        // GetCountiesQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetCountiesQueryHandler(IWebDbContext webDbContext)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
        }

        // Handle metodu, GetCountiesQuery isteğini işler ve ilçelerin listesini döndürür.
        public async Task<List<DistrictDto>> Handle(GetCountiesQuery request, CancellationToken cancellationToken)
        {
            // İlçelerin bulunduğu sorgu oluşturulur, yalnızca belirtilen şehre ait ve onaylanmış (approved) ilçeler alınır.
            var query = _webDbContext.Counties
                .Where(district => district.CitiesKey == request.CityId && district.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved);

            // Sorgu sonucundaki ilçelerin listesi alınır ve asenkron olarak listelenir.
            var counties = await query
                .AsNoTracking() // İzleme devre dışı bırakılır
                .ToListAsync(cancellationToken); // Asenkron olarak liste alınır

            // İlçeler, DistrictDto'ya dönüştürülür, adlarına göre sıralanır ve listelenir.
            return counties
                .Select(district => district.MapToDistrictDto()) // İlçeleri DistrictDto'ya dönüştürür
                .OrderBy(t => t.Name) // İlçeleri ada göre sıralar
                .ToList(); // Listeye dönüştürür
        }
    }
}
