using System.ComponentModel.DataAnnotations;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Employees.Request;

public class EmployeeUpdateDto
{
    internal long customer_id;

    [Required]
    public long company_id { get; set; }
    [Required]
    public string name { get; set; }
    [Required]
    public string surname { get; set; }

    public Gender gender { get; set; }
    [Required]
    public long phone { get; set; }

    public string mail { get; set; }
    public long? phone2 { get; set; }
    [Required]
    public string address { get; set; }
    public string department { get; set; }
    public string? description { get; set; }

    public Status status { get; set; }  
}
