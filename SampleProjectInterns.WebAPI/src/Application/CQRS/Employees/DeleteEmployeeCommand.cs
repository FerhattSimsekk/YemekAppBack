using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Employees;

public record DeleteEmployeeCommand(long Id):IRequest;

public class DeleteEmployeeCommandHandler:IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;

    public DeleteEmployeeCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
    {
        _webDbContext = webDbContext;
        _principal = principal;
    }
    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
            .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
            ?? throw new Exception("User Not Found");



        var employee = await _webDbContext.Employees.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Employee Not found", "Employee");

        //buraya şart eklenecek
        employee.Status = Status.deleted;
        await _webDbContext.SaveChangesAsync(cancellationToken);


        return Unit.Value;
    }
}


