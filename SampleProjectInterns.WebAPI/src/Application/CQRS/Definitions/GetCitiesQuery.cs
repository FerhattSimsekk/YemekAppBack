using Application.Dtos.Cities.Response;         // Şehirlerin yanıt DTO'larını içeren namespace
using Application.Interfaces;                   // Uygulama arayüzleri için gerekli namespace
using Application.Mappers;                      // Veri nesnelerini DTO'lara dönüştürmek için gerekli namespace
using MediatR;                                  // MediatR kütüphanesini kullanmak için gerekli namespace
using Microsoft.EntityFrameworkCore;            // Entity Framework Core ile ilgili namespace'ler

namespace Application.CQRS.Definitions
{
    // GetCitiesQuery, şehirlerin listesini getirmek için kullanılan bir isteği temsil eder.
    public record GetCitiesQuery() : IRequest<List<CityDto>>;

    // GetCitiesQueryHandler, GetCitiesQuery isteğini işleyen bir sınıftır.
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
    {
        private readonly IWebDbContext _webDbContext; // Veritabanı bağlamı için arayüz referansı

        // GetCitiesQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetCitiesQueryHandler(IWebDbContext webDbContext)
        {
            _webDbContext = webDbContext; // Veritabanı bağlamı enjekte edilir
        }

        // Handle metodu, GetCitiesQuery isteğini işler ve şehirlerin listesini döndürür.
        public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            // Şehirlerin bulunduğu sorgu oluşturulur, yalnızca onaylanmış (approved) şehirler alınır.
            var query = _webDbContext.Cities
                .Where(city => city.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved);

            // Sorgu sonucundaki şehirlerin listesi alınır ve asenkron olarak listelenir.
            var cities = await query
                .AsNoTracking() // İzleme devre dışı bırakılır
                .ToListAsync(cancellationToken); // Asenkron olarak liste alınır

            // Şehirler, CityDto'ya dönüştürülür, adlarına göre sıralanır ve listelenir.
            return cities
                .Select(city => city.MapToCityDto()) // Şehirleri CityDto'ya dönüştürür
                .OrderBy(t => t.Name) // Şehirleri ada göre sıralar
                .ToList(); // Listeye dönüştürür
        }
    }
}
