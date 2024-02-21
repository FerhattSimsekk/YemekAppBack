using Application.Dtos.Companies.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Companies
{
    // GetCompanyByIdQuery, bir şirketi ID'ye göre almak için kullanılan bir isteği temsil eder.
    public record GetCompanyByIdQuery(long Id) : IRequest<CompanyDto>;

    // GetCompanyByIdQueryHandler, GetCompanyByIdQuery isteğini işleyen bir sınıftır.
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // GetCompanyByIdQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetCompanyByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, GetCompanyByIdQuery isteğini işler ve CompanyDto döndürür.
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Kullanıcının yetkilendirme seviyesi kontrol edilir, admin olmayanlar yetkilendirme hatası alır.
            var auht = identity.Type;
            if (auht is not AdminAuthorization.admin)
                throw new UnAuthorizedException("Unauthorized access", "Company");

            // ID'ye göre şirket veritabanından alınır, bulunamazsa hata alınır.
            var company = await _webDbContext.Companies.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Company not found", "Company");

            // Şirketin şehir ve ilçe bilgileri alınır ve CompanyDto'ya dönüştürülerek döndürülür.
            var cities = await _webDbContext.Cities.AsNoTracking().FirstOrDefaultAsync(city => city.Key == company.CityId, cancellationToken);
            var counties = await _webDbContext.Counties.AsNoTracking().FirstOrDefaultAsync(county => county.Key == company.CountyId, cancellationToken);

            return company.MapToCompanyDto(cities.Name, counties.Name);
        }
    }
}
