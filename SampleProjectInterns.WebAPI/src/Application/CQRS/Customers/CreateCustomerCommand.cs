using Application.Dtos.Customers.Request;
using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;

namespace Application.CQRS.Customers;

public record CreateCustomerCommand(CustomerCreateDto Customer):IRequest<CustomerDto>;
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public CreateCustomerHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {

        var identity = await _webDbContext.Identities.AsNoTracking()
          .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
          ?? throw new Exception("User not found");


        Customer customer = new()
        {
            Address=request.Customer.address,
            CompanyId=request.Customer.company_id,
            Description=request.Customer.description,
            Gender=request.Customer.gender,
            Mail=request.Customer.mail,
            Phone=request.Customer.phone,
            Status= SampleProjectInterns.Entities.Common.Enums.Status.approved,
            Name=request.Customer.name,
            Surname=request.Customer.surname
            
        };
        await _webDbContext.Customers.AddAsync(customer,cancellationToken);
        await _webDbContext.SaveChangesAsync(cancellationToken);    


        return customer.MapToCustomerDto();
    }
}
