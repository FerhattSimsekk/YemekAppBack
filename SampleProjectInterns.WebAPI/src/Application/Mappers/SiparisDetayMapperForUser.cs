using Application.Dtos.SiparisDetays.Response;
using Application.Dtos.Siparisler.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class SiparisDetayMapperForUser
	{
		public static SiparisDetayDtoForUser MapToSiparisDetayDtoForUser(this SiparisDetay siparisDetay)
		{
			return new SiparisDetayDtoForUser(
				siparisDetay.Id,
				siparisDetay.SiparisId,

				siparisDetay.UrunId,
				siparisDetay.Urun.ResimUrl,
				siparisDetay.Urun.Ad,
				siparisDetay.Adet,
				siparisDetay.Fiyat,
				siparisDetay.Toplam

				);
		}
	}
}
