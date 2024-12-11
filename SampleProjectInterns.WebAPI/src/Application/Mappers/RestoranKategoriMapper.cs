using Application.Dtos.RestoranKategorileri.Response;
using Application.Dtos.Restorans.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class RestoranKategoriMapper
	{
		public static RestoranKategoriDTO MaptoRestoranKategoriDto(this KategoriRestoran KategoriRestoran)
		{
			return new RestoranKategoriDTO(
				KategoriRestoran.Id,
				KategoriRestoran.Ad,
				KategoriRestoran.ResimUrl

				);
		}
	}
}

