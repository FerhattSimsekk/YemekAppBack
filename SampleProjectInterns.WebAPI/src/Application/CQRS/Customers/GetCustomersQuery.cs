using Application.Dtos.Companies.Response;
using Application.Dtos.Customers.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Customers
{
	public record GetCustomersQuery() : IRequest<List<CustomerDto>>;

	public class GetcCustomerQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetcCustomerQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Customer");

			var customers = await _webDbContext.Customers
				.AsNoTracking()
				.OrderByDescending(order => order.CreatedAt)
				.Select(customers => customers.MapToCustomerDto())
				.ToListAsync(cancellationToken);

			return customers;
		}
	}
}
