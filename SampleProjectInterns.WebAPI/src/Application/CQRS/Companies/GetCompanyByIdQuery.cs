using Application.Dtos.Companies.Request;
using Application.Interfaces.Mailing;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Principal;
using System.Text;
using Application.Dtos.Companies.Response;
using Microsoft.EntityFrameworkCore;
using Application.Mappers;

namespace Application.CQRS.Companies;


public record GetCompanyByIdQuery(long Id) : IRequest<CompanyDto>;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetCompanyByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");

        var auht = identity.Type;
        if (auht is not AdminAuthorization.admin)
            throw new UnAuthorizedException("Unauthorized access", "Company");

        var company = await _webDbContext.Companies.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
           ?? throw new NotFoundException($"Company not found", "Company");
        var cities = await _webDbContext.Cities.AsNoTracking().FirstOrDefaultAsync(city => city.Key == company.CityId, cancellationToken);
        var counties = await _webDbContext.Counties.AsNoTracking().FirstOrDefaultAsync(county => county.Key == company.CountyId, cancellationToken);

        return company.MapToCompanyDto(cities.Name, counties.Name); 
    }
}