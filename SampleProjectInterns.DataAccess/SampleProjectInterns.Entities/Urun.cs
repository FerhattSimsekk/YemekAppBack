using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
	public class Urun:BaseEntity
	{
		public long RestoranId { get; set; } // Foreign Key
		public string Ad { get; set; } = null!;
		public decimal Fiyat { get; set; }

		public string? Aciklama { get; set; }
		public string? ResimUrl { get; set; }
		public string Kategori { get; set; } = null!;
		public bool Aktif { get; set; }
		public ICollection<SiparisDetay> SiparisDetaylari { get; set; }

	}
}
