using Application.Dtos.Customers.Response;
using Application.Dtos.Employees.Response;
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

namespace Application.CQRS.Employees
{
	public record GetEmployeesQuery() : IRequest<List<EmployeeDto>>;

	public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetEmployeesQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Employee");

			var employees = await _webDbContext.Employees
				.AsNoTracking()
				.OrderByDescending(order => order.CreatedAt)
				.Select(customers => customers.MapToEmployeeDto())
				.ToListAsync(cancellationToken);

			return employees;
		}
	}
}
