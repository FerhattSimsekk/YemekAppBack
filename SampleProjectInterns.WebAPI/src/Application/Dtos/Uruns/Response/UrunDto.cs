using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Uruns.Response
{
	public record UrunDto(
	long id,
	long restoranId,
	string ad,
	decimal fiyat,
	string? aciklama,
	string? resimUrl,
	string kategori,
	Status status,
	DateTime created,
	DateTime? updated
);
}
