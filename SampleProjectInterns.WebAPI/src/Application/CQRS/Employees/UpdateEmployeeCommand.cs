using Application.Dtos.Employees.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Employees;

public record UpdateEmployeeCommand(EmployeeUpdateDto Employee,long EmployeesId):IRequest;

public class UpdateEmployeCommandHandler:IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public UpdateEmployeCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<Unit>Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
        .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
        ?? throw new Exception("User not found");

        var employee = await _webDbContext.Employees.FirstOrDefaultAsync(i => i.Id == request.EmployeesId, cancellationToken) ??
            throw new NotFoundException($"{request.Employee.name} not found", "Employee");


        employee.Name = request.Employee.name;
        employee.Surname = request.Employee.surname;
        employee.Gender = request.Employee.gender;
        employee.Mail = request.Employee.mail;
        employee.Phone = request.Employee.phone;
        employee.Mail = request.Employee.mail;
        employee.Phone2 = request.Employee.phone2;
        employee.Address = request.Employee.address;
        employee.Department = request.Employee.department;
        employee.Description = request.Employee.description;
        employee.Status = request.Employee.status;
  
        await _webDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
