using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities
{
	public class Odeme:BaseEntity
	{
		public long SiparisId { get; set; }
		public OdemeTipi Tip { get; set; } // Kredi Kartı, Nakit vb.
		public bool OdendiMi { get; set; }
		public DateTime OdemeTarihi { get; set; }

	}
}
