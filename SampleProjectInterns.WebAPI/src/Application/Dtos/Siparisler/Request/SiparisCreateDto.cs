using Application.Dtos.SiparisDetays;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Siparisler.Request
{
	public class SiparisCreateDto
	{
		public long RestoranId { get; set; }
		public long AdresId { get; set; }

		public decimal ToplamTutar { get; set; }
		public ICollection<SiparisDetayCreateDto> SiparisDetaylari { get; set; }
	}
}
