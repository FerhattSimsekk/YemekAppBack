using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Uruns.Request
{
	public class UrunUpdateDto
	{
		public long restoranId { get; set; } // Foreign Key
		public string ad { get; set; } = null!;
		public decimal fiyat { get; set; }

		public string? aciklama { get; set; }
		public IFormFile? resimUrl { get; set; }
		public string kategori { get; set; } = null!;
		public bool aktif { get; set; }
		public Status status { get; set; } 

	}
}
