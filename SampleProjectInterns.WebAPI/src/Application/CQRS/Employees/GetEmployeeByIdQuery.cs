using Application.Dtos.Customers.Response;
using Application.Dtos.Employees.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employees
{
	public record GetEmployeeByIdQuery(long Id) : IRequest<EmployeeDto>;

	public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetEmployeeByIdQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
			 .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			 ?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Employee");




			var employee = await _webDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
			   ?? throw new NotFoundException($"Customer not found", "Employee");



			return employee.MapToEmployeeDto();
		}
	}
}
