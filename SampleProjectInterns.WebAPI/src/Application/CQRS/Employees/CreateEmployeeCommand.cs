using Application.Dtos.Employees.Request;
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
	public record CreateEmployeeCommand(EmployeeCreateDto Employee) : IRequest<EmployeeDto>;

	public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public CreateEmployeeHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");
			var auht = identity.Type;
			if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Employee");

			var employee = new Employee
			{
				Name = request.Employee.name,
				Surname = request.Employee.surname,
				Gender = request.Employee.gender,
				Phone = request.Employee.phone,
				Mail = request.Employee.mail,
				Description = request.Employee.description,
				Department = request.Employee.department,
				Address = request.Employee.address,
				Status = SampleProjectInterns.Entities.Common.Enums.Status.approved,
				CompanyId = identity.CompanyId
			};

			await _webDbContext.Employees.AddAsync(employee, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);

			return employee.MapToEmployeeDto();
		}
	}
}
