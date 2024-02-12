using Application.Dtos.Identities.Request;
using Application.Dtos.Identities.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Identities;
public record IdentityAuthenticationQuery(IdentityVerifyDto Identity) : IRequest<IdentityDto>;

public class IdentityAuthenticationQueryHandler : IRequestHandler<IdentityAuthenticationQuery, IdentityDto?>
{
    private readonly IWebDbContext _webDbContext;

    public IdentityAuthenticationQueryHandler(IWebDbContext webDbContext)
    {
        _webDbContext = webDbContext;
    }

    public async Task<IdentityDto?> Handle(IdentityAuthenticationQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
            .FirstOrDefaultAsync(identity => identity.Email == request.Identity.Email&&identity.Status== SampleProjectInterns.Entities.Common.Enums.Status.approved, cancellationToken);

        if (identity == null) return null;
        
        var isPasswordVerified = BCrypt.Net.BCrypt.HashPassword(request.Identity.Password, identity.Salt)
            == identity.Password;

        return isPasswordVerified ? identity.MapToIdentityDto() : null;
    }
}