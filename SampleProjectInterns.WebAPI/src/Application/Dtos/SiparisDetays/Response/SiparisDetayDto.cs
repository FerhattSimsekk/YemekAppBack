using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.SiparisDetays.Response
{
	
	
		public record SiparisDetayDto
	(
			long Id,
		 long SiparisId ,
	 long UrunId ,
	 int Adet ,
	 decimal Fiyat ,
	 decimal Toplam ,
	 Status Status,
	 DateTime created,
	DateTime? updated
	);
	}

