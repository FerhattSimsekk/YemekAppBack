using Application.Dtos.Customers.Response;
using Application.Dtos.Employees.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Employees;

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



        var employee = await _webDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
           ?? throw new NotFoundException($"Employee not found", "Customer");



        return employee.MapToEmployeeDto();
    }
}