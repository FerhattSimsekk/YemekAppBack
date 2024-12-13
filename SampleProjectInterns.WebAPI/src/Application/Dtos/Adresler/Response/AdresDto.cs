using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Adresler.Response
{
	
		public record AdresDto(
	long id,
	long identityId,
	string baslik,
	string AdresBilgisi,
	int cityId,
	int countyId,
	Status status,
	DateTime created,
	DateTime? updated
);
	
}
