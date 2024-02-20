using Application.Dtos.Employees.Response;
using SampleProjectInterns.Entities;

namespace Application.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto MapToEmployeeDto(this Employee employee)
        {
            return new EmployeeDto(
                employee.Id,
                employee.CompanyId,
                employee.Name,
                employee.Surname,
                employee.Gender,
                employee.Phone,
                employee.Mail,
                employee.Phone2,
                employee.Address,
                employee.Department,
                employee.Description,
                employee.CreatedAt,
                employee.UpdatedAt,
                employee.Status
                );
        }
    }
}
