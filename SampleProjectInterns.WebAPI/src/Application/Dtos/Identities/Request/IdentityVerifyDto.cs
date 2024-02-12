using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Identities.Request;
public class IdentityVerifyDto
{
    [MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MinLength(6)]
    [MaxLength(512)]
    public string Password { get; set; } = null!;
}
