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
		public static SiparisDto MapToSiparisDto(this Siparis siparis)
		{
			return new SiparisDto(
				siparis.Id,
				siparis.IdentityId,
				siparis.RestoranId,
				siparis.Durum,
				siparis.ToplamTutar,
				siparis.TeslimTarihi,
				siparis.SiparisDetaylari,
				siparis.Yorumlar,
				siparis.Status,
				siparis.CreatedAt,
				siparis.UpdatedAt
				);
		}
	}
}
