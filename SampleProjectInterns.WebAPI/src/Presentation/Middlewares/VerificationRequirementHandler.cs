using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.CQRS.Identities;

namespace SampleProjectInterns.WebAPI.Presentation.Middlewares;

public class VerificationRequirementHandler : AuthorizationHandler<VerificationRequirement>
{
    private readonly IMediator _mediator;

    public VerificationRequirementHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, VerificationRequirement requirement)
    {
        var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;

        var isVerified = isAuthenticated
            && await _mediator.Send(new IdentityVerificationQuery(context.User!.Identity!.Name!));

        if (isVerified) 
            context.Succeed(requirement); 
    }
}