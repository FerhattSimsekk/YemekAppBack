using SampleProjectInterns.Entities.Common;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities;

public class Identity : BaseEntity
{
    public long? RestoranId { get; set; } 
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public AdminAuthorization Type { get; set; }
	public ICollection<Siparis>? Siparisler { get; set; }
	public ICollection<Adres>? Adresler { get; set; }  // Birden fazla adres olabi
	public ICollection<Yorum>? Yorumlar { get; set; } // Kullanıcının yaptığı yorumlar
}
