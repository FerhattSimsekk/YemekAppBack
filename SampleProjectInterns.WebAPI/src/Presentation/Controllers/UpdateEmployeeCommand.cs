using Application.Dtos.Employees.Request;
using MediatR;

namespace Presentation.Controllers
{
    internal class UpdateEmployeeCommand : IRequest<object?>
    {
        private EmployeeUpdateDto employee;
        private long id;

        public UpdateEmployeeCommand(EmployeeUpdateDto employee, long id)
        {
            this.employee = employee;
            this.id = id;
        }
    }
}