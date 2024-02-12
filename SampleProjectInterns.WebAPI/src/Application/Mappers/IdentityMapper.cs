using Application.Dtos.Identities.Response;
using SampleProjectInterns.Entities;

namespace Application.Mappers;

public static class IdentityMapper
{
    public static IdentityDto MapToIdentityDto(this Identity identity)
    {
        return new IdentityDto(
            identity.Email,
            identity.Name,
            identity.LastName,
            identity.Type.ToString());
    }
}