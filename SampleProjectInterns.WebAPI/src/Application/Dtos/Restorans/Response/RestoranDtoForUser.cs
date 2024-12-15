using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Restorans.Response
{
	public record RestoranDtoForUser(
	long Id,
	string Ad,
	string ResimUrl
);
}
