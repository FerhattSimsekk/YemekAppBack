using Application.Dtos.Cities.Response;
using Application.Dtos.Counties.Response;
using SampleProjectInterns.Entities;

namespace Application.Mappers;

public static class DefinitionMapper
{
    public static CityDto MapToCityDto(this City city)
    {
        return new CityDto
        (
            Id: city.Key,
            Name: city.Name
        );
    }
    public static DistrictDto MapToDistrictDto(this County district)
    {
        return new DistrictDto
        (
           Id: district.Key,
           Name: district.Name
        );
    }
}
