using Application.Dtos.Customers.Request;
using Application.Dtos.Employees.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;
namespace Application.CQRS.Employees
{
	public record UpdateEmployeeCommand(EmployeeUpdateDto Employee, long EmployeeId) : IRequest;
	public class UpdateCustomerCommandHandler : IRequestHandler<UpdateEmployeeCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public UpdateCustomerCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
		{

			var identity = await _webDbContext.Identities.AsNoTracking()
			.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Employee");

			var employee = await _webDbContext.Employees.FirstOrDefaultAsync(i => i.Id == request.EmployeeId, cancellationToken)
				?? throw new NotFoundException($"{request.Employee.name} not found", "Employee");


			employee.Address = request.Employee.address;
			employee.Description = request.Employee.description;
			employee.Mail = request.Employee.mail;
			employee.Name = request.Employee.name;
			employee.Surname = request.Employee.surname;
			employee.Gender = request.Employee.gender;
			employee.Phone = request.Employee.phone;
			employee.Status = request.Employee.status;
			employee.UpdatedAt = DateTime.Now;
			

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

		}
	}
}
