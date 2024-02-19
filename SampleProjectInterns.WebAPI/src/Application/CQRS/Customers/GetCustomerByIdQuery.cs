using Application.CQRS.Companies;
using Application.Dtos.Companies.Response;
using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Diagnostics.Metrics;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Customers;

public record GetCustomerByIdQuery(long Id) : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetCustomerByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");



        var customer = await _webDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
           ?? throw new NotFoundException($"Customer not found", "Customer");

      

        return customer.MapToCustomerDto();
    }
}
