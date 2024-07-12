using SampleProjectInterns.Entities.Common;

namespace SampleProjectInterns.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? Logo { get; set; } = null!;
    public string? Host { get; set; } = null!;
    public string PageTitle { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string? TaxNumber { get; set; }
    public string? TaxAdministration { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
    public int CountyId { get; set; } 
    public ICollection<Identity> Identities { get; set; }
       = new List<Identity>();

}
