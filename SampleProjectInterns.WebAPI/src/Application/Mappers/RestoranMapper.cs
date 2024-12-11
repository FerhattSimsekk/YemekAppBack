using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Application.Dtos.Restorans.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Mappers
{
	public static class RestoranMapper
	{
		public static RestoranDto MaptoRestoranDto(this Restoran restoran,string? kategoriAdi)
		{
			return new RestoranDto(
				restoran.Id,
				restoran.Ad,
				restoran.Telefon,
				restoran.Adres,
				restoran.Hakkinda,
				restoran.ResimUrl,
				restoran.AcikMi,
				restoran.CalismaSaatleri,
				restoran.TahminiTeslimatSüresi,
				restoran.KategoriRestoranId,
				kategoriAdi,
				restoran.Status,
				restoran.CreatedAt,
				restoran.UpdatedAt

				);
		}
	}
}
