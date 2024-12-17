using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
	public class Yorum:BaseEntity
	{
		public long SiparisId { get; set; } // Foreign Key
		public long IdentityId { get; set; } // Foreign Key
		public Identity Identity { get; set; }
		public int Derecelendirme { get; set; } // 1-5 arası puan
		public string YorumMetni { get; set; }
		public DateTime YorumTarihi { get; set; }

	}
}
