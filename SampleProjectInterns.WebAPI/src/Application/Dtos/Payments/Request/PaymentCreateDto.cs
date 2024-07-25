using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.Threading.Tasks;

namespace Application.Dtos.Payments.Request
{
	public class PaymentCreateDto
	{
		[Required]
		public long company_id { get; set; }
		[Required]

		public long customer_id { get; set; }
		[Required]

		public decimal price { get; set; }
		[Required]

		public string bill_number { get; set; }
		public string? description { get; set; }
		public DateTime last_payment_date { get; set; }
	}
}
