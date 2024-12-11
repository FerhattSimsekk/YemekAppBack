using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.RestoranKategorileri.Response
{

	public record RestoranKategoriDTO(
	long id,
	string ad,
	string? resimUrl
);
}

