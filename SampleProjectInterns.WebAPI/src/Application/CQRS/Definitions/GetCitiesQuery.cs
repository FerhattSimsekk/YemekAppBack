using Application.Dtos.Cities.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Definitions;

public record GetCitiesQuery() : IRequest<List<CityDto>>;
public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
{
    private readonly IWebDbContext _webDbContext;

    public GetCitiesQueryHandler(IWebDbContext webDbContext)
    {
        _webDbContext = webDbContext;
    }

    public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _webDbContext.Cities
            .Where(city => city.Status ==  SampleProjectInterns.Entities.Common.Enums.Status.approved);

        var cities = await query
               .AsNoTracking()
               .ToListAsync(cancellationToken);

        return cities
            .Select(city => city.MapToCityDto())
            .OrderBy(t => t.Name)
            .ToList();
    }
}