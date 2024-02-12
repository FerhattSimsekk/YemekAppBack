using Application.Dtos.Companies.Response;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Application.Mappers;

namespace Application.CQRS.Companies;

public record GetCompaniesQuery() : IRequest<List<CompanyDto>>;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<CompanyDto>>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetCompaniesQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<List<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");

        var auht = identity.Type;
        if (auht is not AdminAuthorization.admin)
            throw new UnAuthorizedException("Unauthorized access", "Company"); 


        var company = await _webDbContext.Companies 
            .AsNoTracking()
            .OrderByDescending(order=>order.CreatedAt)
            .Select(companies => companies.MapToCompanyDto("", ""))
            .ToListAsync(cancellationToken);

       
        return company; 
    }
}