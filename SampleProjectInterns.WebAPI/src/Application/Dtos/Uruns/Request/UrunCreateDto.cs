using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Uruns.Request
{
	public class UrunCreateDto
	{
		public long restoranId { get; set; } // Foreign Key
		public string ad { get; set; } = null!;
		public decimal fiyat { get; set; }

		public string? aciklama { get; set; }
		public string? resimUrl { get; set; }
		public string kategori { get; set; } = null!;
		public bool aktif { get; set; }
	}
}
