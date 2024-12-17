using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Yorumlar.Request
{
	public class YorumCreateDto
	{

		public long SiparisId { get; set; } // Foreign Key
		public int Derecelendirme { get; set; } // 1-5 arası puan
		public string YorumMetni { get; set; }
	}
}
