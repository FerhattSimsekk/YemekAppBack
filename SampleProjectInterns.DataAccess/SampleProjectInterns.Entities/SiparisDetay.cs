using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
	public class SiparisDetay:BaseEntity
	{
		public long SiparisId { get; set; }
		public long UrunId { get; set; }
		public int Adet { get; set; }
		public decimal Fiyat { get; set; } // Sipariş anındaki ürün fiyatı
		public decimal Toplam { get; set; } 
	}
}
