using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Companies.Request;

public class CompanyUpdateDto
{
    [Required]
    [MaxLength(512)]
    public string name { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    [EmailAddress]
    public string email { get; set; } = null!;

    [Required] 
    public long phone { get; set; }

    public string? description { get; set; } = null!;
   
    public IFormFile? logo { get; set; }

    public string? host { get; set; } = null!;

    [Required]
    public string page_title { get; set; } = null!;

    [Required]
    public string short_name { get; set; } = null!;
    public int city_id { get; set; }
    public int county_id { get; set; }

    public string? tax_number { get; set; }

    public string? tax_administration { get; set; }

    public string? address { get; set; }
    [Required]
    public SampleProjectInterns.Entities.Common.Enums.Status Status { get; set; }
}
