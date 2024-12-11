using Application.Dtos.Restorans.Response;
using Application.Dtos.Uruns.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class UrunMapper
	{
		public static UrunDto MapToUrunDto(this Urun urun)
		{
			return new UrunDto(
				urun.Id,
				urun.RestoranId,
				urun.Ad,
				urun.Fiyat,
				urun.Aciklama,
				urun.ResimUrl,
				urun.Kategori,
				urun.Status,
				urun.CreatedAt,
				urun.UpdatedAt
				);
		}
	}
}
