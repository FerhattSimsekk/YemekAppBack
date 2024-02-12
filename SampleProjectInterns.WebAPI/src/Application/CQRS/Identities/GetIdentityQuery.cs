using Application.Dtos.Identities.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Identities;
public record GetIdentityQuery() : IRequest<IdentityDto>;

public class GetIdentityQueryHandler : IRequestHandler<GetIdentityQuery, IdentityDto?>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetIdentityQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<IdentityDto?> Handle(GetIdentityQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
          .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name && identity.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved, cancellationToken)
          ?? throw new Exception("User not found");

        if (identity == null) return null;
        return identity.MapToIdentityDto();
    }
}