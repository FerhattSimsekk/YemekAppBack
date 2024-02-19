using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Customers;

public record DeleteCustomerCommand(long Id) : IRequest;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public DeleteCustomerCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
            .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
            ?? throw new Exception("User Not Found");


        
        var customer = await _webDbContext.Customers.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Customer Not found", "Customer");

        //buraya şart eklenecek
        customer.Status = Status.deleted;
        await _webDbContext.SaveChangesAsync(cancellationToken);


        return Unit.Value;
    }
}
