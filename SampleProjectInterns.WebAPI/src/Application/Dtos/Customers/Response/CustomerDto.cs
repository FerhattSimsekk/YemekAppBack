using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Customers.Response
{
	public record CustomerDto
	(
		long id,

		long company_id,

		string name,
		
		string surname,
		Gender gender,
		long? phone,

		string mail,

		string address,

		string? description,
		DateTime created_at,
		DateTime? updated_at,
		Status status
	);
}
