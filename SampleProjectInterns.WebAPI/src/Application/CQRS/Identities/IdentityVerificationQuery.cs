using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Identities;

public record IdentityVerificationQuery(string UserName) : IRequest<bool>;

public class IdentityVerificationQueryHandler : IRequestHandler<IdentityVerificationQuery, bool>
{
    private readonly IWebDbContext _webDbContext;

    public IdentityVerificationQueryHandler(IWebDbContext webDbContext)
    {
        _webDbContext = webDbContext;
    }

    public async Task<bool> Handle(IdentityVerificationQuery request, CancellationToken cancellationToken)
    {
        var isVerified = await _webDbContext.Identities
            .Where(identity => identity.Email == request.UserName && identity.Status == SampleProjectInterns.Entities.Common.Enums.Status.approved)
            //.Select(identity => identity.IsVerified as bool?)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (isVerified is not null)
            return true;
        return false;
        // return isVerified == true;
    }
}