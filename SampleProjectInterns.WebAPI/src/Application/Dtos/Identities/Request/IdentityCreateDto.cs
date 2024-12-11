using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Identities.Request;

public class IdentityCreateDto
{
   
    public long? RestoranId { get; set; }

    [MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(512)]
    public string Name { get; set; } = null!;

    [MaxLength(512)]
    public string Surname { get; set; } = null!;

    //[MinLength(6)]
    //[MaxLength(512)]
    //public string Password { get; set; } = null!;
    [Required]
    public int Type { get; set; }
}
