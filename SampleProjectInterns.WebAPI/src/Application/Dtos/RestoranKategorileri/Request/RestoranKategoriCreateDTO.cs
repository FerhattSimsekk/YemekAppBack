using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RestoranKategorileri.Request
{
	public class RestoranKategoriCreateDTO
	{
		public string ad { get; set; } = null!;
		public IFormFile? resimUrl { get; set; }
	}
}
