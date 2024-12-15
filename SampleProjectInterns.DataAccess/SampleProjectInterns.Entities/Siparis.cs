using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities
{
	public class Siparis : BaseEntity
	{
		
		public long IdentityId { get; set; }
		public long RestoranId { get; set; }
		public Restoran Restoran { get; set; }

		public SiparisDurumu Durum { get; set; } // Sipariş durumu (Hazırlanıyor, Yolda, Teslim Edildi)
		public long AdresId { get; set; } 
		public decimal ToplamTutar { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public DateTime? TeslimTarihi { get; set; }
		public ICollection<SiparisDetay> SiparisDetaylari { get; set; }
		public ICollection<Yorum> Yorumlar { get; set; } // Siparişe yapılmış yorumlar
	}
}
