using Application.Dtos.Uruns.Response;
using Application.Dtos.Yorumlar.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class YorumMapper
	{
		public static YorumDto MapToYorumDto(this Yorum yorum)
		{
			return new YorumDto(
				yorum.Id,
				yorum.Identity.MapToIdentityDto(),
				yorum.Derecelendirme,
				yorum.YorumMetni,
				yorum.Status,
				yorum.CreatedAt,
				yorum.UpdatedAt
				);
		}
	}
}
