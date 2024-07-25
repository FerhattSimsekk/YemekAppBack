using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Payments.Response
{
	public record PaymentDto(
	long id,
	long company_id,
	long customer_id,
	decimal price,
	string bill_number,
	string? description,
	DateTime last_payment_date,
	DateTime created_at,
	DateTime? updated_at,
	Status status
	);
}
