using SampleProjectInterns.Entities.Common;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities;

public class Identity : BaseEntity
{
    public long CompanyId { get; set; } 
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public AdminAuthorization Type { get; set; } 
}
