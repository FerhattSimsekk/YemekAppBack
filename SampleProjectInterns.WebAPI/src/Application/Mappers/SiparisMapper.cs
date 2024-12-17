using Application.Dtos.Restorans.Response;
using Application.Dtos.Siparisler.Response;
using Application.Dtos.Uruns.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class SiparisMapper
	{
		public static SiparisDtoForUser MapToSiparisDto(this Siparis siparis,int toplamAdet)
		{
			return new SiparisDtoForUser(
				siparis.Id,
				siparis.IdentityId,
				siparis.RestoranId,
				siparis.Durum,
				siparis.ToplamTutar,
				siparis.TeslimTarihi,
				siparis.SiparisDetaylari.Select(x => x.MapToSiparisDetayDtoForUser()).ToList(),
				siparis.Status,
				siparis.CreatedAt,
				siparis.UpdatedAt,
				siparis.Restoran.MapToRestoranDtoForUser(),
				toplamAdet,
				siparis.yorumYapildiMi
				


				);
		}
	}
	
}
