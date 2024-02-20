using Application.Dtos.Companies.Response;
using Application.Dtos.Employees.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Employees;

public record GetEmployeeQuery():IRequest<List<EmployeeDto>>;
public class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, List<EmployeeDto>>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public GetEmployeeHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
          .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
          ?? throw new Exception("User not found");

        var employee = await _webDbContext.Employees.AsNoTracking()
            .OrderByDescending(order => order.CreatedAt)
            .Select(employee => employee
            .MapToEmployeeDto())
            .ToListAsync(cancellationToken);

        return employee;
    }
}
