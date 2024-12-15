using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.SiparisDetays.Response
{
	public record SiparisDetayDtoForUser
	(
			long Id,
		 long SiparisId,
	 long UrunId,
	 string resimUrl,
	 string urunAd,
	 int Adet,
	 decimal Fiyat,
	 decimal Toplam
	);
}
