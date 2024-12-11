using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Restorans.Response
{
	public record RestoranDto(
	long id,
	string ad,
	string telefon,
	string adres,
	string hakkinda,
	string? resimUrl,
	bool acikMi,
	string? calismaSaatleri,
	string? tahminiTeslimatSüresi,
	int KategoriRestoranId,
	string kategorAdi,
	Status status,
	DateTime created,
	DateTime? updated
);
}
