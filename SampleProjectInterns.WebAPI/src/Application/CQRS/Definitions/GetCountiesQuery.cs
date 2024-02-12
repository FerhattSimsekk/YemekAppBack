using Application.Dtos.Counties.Response;
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
public record GetCountiesQuery(int CityId) : IRequest<List<DistrictDto>>;
public class GetCountiesQueryHandler : IRequestHandler<GetCountiesQuery, List<DistrictDto>>
{
    private readonly IWebDbContext _webDbContext;

    public GetCountiesQueryHandler(IWebDbContext webDbContext)
    {
        _webDbContext = webDbContext;
    }

    public async Task<List<DistrictDto>> Handle(GetCountiesQuery request, CancellationToken cancellationToken)
    {
        var query = _webDbContext.Counties
            .Where(district => district.CitiesKey == request.CityId && district.Status ==  SampleProjectInterns.Entities.Common.Enums.Status.approved);

        var counties = await query
               .AsNoTracking()
               .ToListAsync(cancellationToken);

        return counties
            .Select(district => district.MapToDistrictDto())
            .OrderBy(t => t.Name)
            .ToList();
    }
}
