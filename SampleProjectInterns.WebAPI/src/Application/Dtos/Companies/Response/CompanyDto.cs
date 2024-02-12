using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Companies.Response;

public record CompanyDto(
    long id,
    string name,
    string email,
    string phone,
    string? description,
    string? logo,
    string? host,
    string page_title,
    string short_name,
    string? tax_number,
    string? tax_administration,
    string? address,
    int city_id,
    string city_name,
    int county_id,
    string county_name,
    DateTime created,
    DateTime? updated,
    Status status
    ); 