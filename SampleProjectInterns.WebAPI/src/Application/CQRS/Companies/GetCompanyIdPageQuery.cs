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


public record GetCompanyIdPageQuery() : IRequest<List<CompanyIdPageTitleDto>>;

public class GetCompanyIdPageQueryHandler : IRequestHandler<GetCompanyIdPageQuery, List<CompanyIdPageTitleDto>>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetCompanyIdPageQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<List<CompanyIdPageTitleDto>> Handle(GetCompanyIdPageQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");

        var auht = identity.Type;
        //if (auht is not AdminAuthorization.admin)
        //    throw new UnAuthorizedException("Unauthorized access", "Company");

        var company = await _webDbContext.Companies.Where(x => x.Status == Status.approved).AsNoTracking().ToListAsync(cancellationToken)
           ?? throw new NotFoundException($"Company not found", "Company");

        return company.Select(com => com.MapToCompanyIdPageTitleDto()).ToList();
    }
}