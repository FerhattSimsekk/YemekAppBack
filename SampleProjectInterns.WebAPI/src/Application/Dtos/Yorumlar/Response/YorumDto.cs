using Application.Dtos.Identities.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Yorumlar.Response
{
	public record YorumDto(
long id,
IdentityDto Identities,
int derecelendirme,
string yorumMetni,
Status status,
DateTime created,
DateTime? updated
);
}
