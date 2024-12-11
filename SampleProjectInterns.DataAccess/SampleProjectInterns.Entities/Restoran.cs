using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
	public class Restoran:BaseEntity
	{
		public string Ad { get; set; } = null!;
		public string Telefon { get; set; } = null!;
		public string Adres { get; set; } = null!;
		public string Hakkinda { get; set; } = null!;
		public string? ResimUrl { get; set; }
		public bool AcikMi { get; set; }
		public string? CalismaSaatleri { get; set; }
		public string? TahminiTeslimatSüresi { get; set; }
        public int KategoriRestoranId { get; set; }

        public ICollection<Identity> Identities { get; set; }
	  = new List<Identity>();
		public ICollection<Urun> Urunler { get; set; }
		public ICollection<Siparis> Siparisler { get; set; }
	}
}
