using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Employees.Response;

public record EmployeeDto(
    long id,
    long company_id,
    string name,
    string surname,
    Gender gender,
    long phone,
    string mail,
    long? phone2,
    string address,
    string department,
    string? description,
    DateTime created_at,
    DateTime? updated_at,
    Status status


    );
