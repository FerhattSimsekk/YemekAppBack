using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Siparisler.Response
{
	public record SiparisDto
	(
		long Id,
		 long IdentityId,
		 long RestoranId ,
		 SiparisDurumu Durum,
		 decimal ToplamTutar ,
		 DateTime? TeslimTarihi,
		 ICollection<SiparisDetay> SiparisDetaylari ,
		 ICollection<Yorum>? Yorumlar ,
		 Status status,
	DateTime created,
	DateTime? updated
	);
}
