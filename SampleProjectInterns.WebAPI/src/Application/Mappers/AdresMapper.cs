using Application.Dtos.Adresler.Response;
using Application.Dtos.SiparisDetays.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class AdresMapper
	{
		public static AdresDto MapToAdresDto(this Adres adres)
		{
			return new AdresDto(
				adres.Id,
				adres.IdentityId,

				adres.AdresBilgisi,
				adres.Baslik,
				adres.CityId,
				adres.CountyId,
				adres.Status,
				adres.CreatedAt,
				adres.UpdatedAt
				

				);
		}
	}
}
