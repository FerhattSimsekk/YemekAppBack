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
	public static class SiparisDetayMapper
	{
		public static SiparisDetayDto MapToSiparisDetayDto(this SiparisDetay siparisDetay)
		{
			return new SiparisDetayDto(
				siparisDetay.Id,
				siparisDetay.SiparisId,

				siparisDetay.UrunId,
				siparisDetay.Adet,
				siparisDetay.Fiyat,
				siparisDetay.Toplam,
				siparisDetay.Status,
				siparisDetay.CreatedAt,
				siparisDetay.UpdatedAt
				
				);
		}
	}
}
