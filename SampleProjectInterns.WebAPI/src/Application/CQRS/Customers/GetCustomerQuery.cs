using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Customers
{
    public record GetCustomerQuery() : IRequest<List<CustomerDto>>;


    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, List<CustomerDto>>
    {
        private readonly IWebDbContext _webDbContext; 
        private readonly IPrincipal _principal; 

        
        public GetCustomerHandler(IWebDbContext webDbContext, IPrincipal principal)
        {
            _webDbContext = webDbContext; 
            _principal = principal; 
        }

        
        public async Task<List<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
           
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

         
            var customers = await _webDbContext.Customers.AsNoTracking()
                .OrderByDescending(order => order.CreatedAt)
                .Select(customer => customer.MapToCustomerDto())
                .ToListAsync(cancellationToken);

            return customers;
        }
    }
}
