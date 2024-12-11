using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.SiparisDetays
{
	public class SiparisDetayCreateDto
	{
		public long UrunId { get; set; }
		public int Adet { get; set; }
		public decimal Fiyat { get; set; } 
		public decimal Toplam { get; set; }
	}
}
