namespace Application.Dtos.Identities.Response;
public record IdentityDto(
    string Email,
    string? Name,
    string? LastName, 
    string Role);
