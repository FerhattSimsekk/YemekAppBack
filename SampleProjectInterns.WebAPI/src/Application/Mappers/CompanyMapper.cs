using Application.Dtos.Companies.Response;
using SampleProjectInterns.Entities;
using System.Xml.Linq;

namespace Application.Mappers;

public static class CompanyMapper
{
    public static CompanyDto MapToCompanyDto(this Company company,string city_name,string county_name)
    {
        return new CompanyDto(
            company.Id,
            company.Name,
            company.Email,
            company.Phone,
            company.Description,
            (company.Logo==""|| company.Logo == null) ? "Shared/musteri_logo.png":company.Logo,
            company.Host,
            company.PageTitle,
            company.ShortName,
            company.TaxNumber,
            company.TaxAdministration,
            company.Address,
            company.CityId,
            city_name,
            company.CountyId,
            county_name,
            company.CreatedAt,
            company.UpdatedAt,
            company.Status
            );
    }
    public static CompanyIdPageTitleDto MapToCompanyIdPageTitleDto(this Company company)
    {
        return new CompanyIdPageTitleDto(
            company.Id,
            company.PageTitle);
    }
}
