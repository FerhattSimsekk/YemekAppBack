using Application.Dtos.Companies.Response;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Application.Mappers;

namespace Application.CQRS.Companies
{
    // GetCompaniesQuery, tüm şirketleri almak için kullanılan bir isteği temsil eder.
    public record GetCompaniesQuery() : IRequest<List<CompanyDto>>;

    // GetCompaniesQueryHandler, GetCompaniesQuery isteğini işleyen bir sınıftır.
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<CompanyDto>>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;

        // GetCompaniesQueryHandler, gerekli bağımlılıkları alarak oluşturulur.
        public GetCompaniesQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext;
            _principal = principal;
        }

        // Handle metodu, GetCompaniesQuery isteğini işler ve bir liste olarak CompanyDto'ları döndürür.
        public async Task<List<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Kullanıcının yetkilendirme seviyesi kontrol edilir, admin olmayanlar yetkilendirme hatası alır.
            var auht = identity.Type;
            if (auht is not AdminAuthorization.admin)
                throw new UnAuthorizedException("Unauthorized access", "Company");

            // Tüm şirketler alınır, oluşturulma tarihine göre sıralanır ve CompanyDto'ya dönüştürülür.
            var companies = await _webDbContext.Companies
                .AsNoTracking()
                .OrderByDescending(order => order.CreatedAt)
                .Select(companies => companies.MapToCompanyDto("", ""))
                .ToListAsync(cancellationToken);

            return companies;
        }
    }
}
