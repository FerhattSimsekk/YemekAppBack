using Application.Dtos.Restorans.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class RestoranMapperForUser
	{
		public static RestoranDtoForUser MapToRestoranDtoForUser(this Restoran restoran)
		{
			return new RestoranDtoForUser(
				restoran.Id,
				restoran.Ad,
				restoran.ResimUrl
			);
		}
	}
}
