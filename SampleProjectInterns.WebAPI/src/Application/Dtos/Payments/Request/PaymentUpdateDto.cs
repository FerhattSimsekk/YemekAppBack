using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Payments.Request
{
	public class PaymentUpdateDto
	{
		[Required]
		public long company_id { get; set; }
		[Required]

		public long customer_id { get; set; }
		public decimal price { get; set; }
		[Required]

		public string bill_number { get; set; }
		public string? description { get; set; }
		public DateTime last_payment_date { get; set; }
		public Status status { get; set; }
	}
}
