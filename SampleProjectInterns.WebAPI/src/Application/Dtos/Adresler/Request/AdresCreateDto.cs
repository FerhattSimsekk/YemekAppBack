using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Adresler.Request
{
	public class AdresCreateDto
	{
		public string Baslik { get; set; } = null!;// Ev, İş, vb.
		public string AdresBilgisi { get; set; } = null!;
		public int CityId { get; set; }
		public int CountyId { get; set; }
	}
}
