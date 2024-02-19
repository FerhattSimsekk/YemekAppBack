using System.ComponentModel.DataAnnotations;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Customers.Request;

public class CustomerCreateDto
{
    [Required]
    public long company_id { get; set; }
    [Required]
    public string name { get; set; }
    [Required]
    public string surname { get; set; } 
    public Gender gender { get; set; }
    public long? phone { get; set; }
    public string? mail { get; set; }
    [Required]
    public string address { get; set; } 
    public string? description { get; set; }
}
