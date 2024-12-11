using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Siparisler.Request
{
	public class SiparisUpdateDto
	{
		public long IdentityId { get; set; }
		public long RestoranId { get; set; }
		public SiparisDurumu Durum { get; set; } // Sipariş durumu (Hazırlanıyor, Yolda, Teslim Edildi)
		public decimal ToplamTutar { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public DateTime? TeslimTarihi { get; set; }
		public long? OdemeId { get; set; }
		public ICollection<SiparisDetay> SiparisDetaylari { get; set; }
		public ICollection<Yorum> Yorumlar { get; set; } // Siparişe yapılmış yorumlar
		public Status status { get; set; }
	}
}
