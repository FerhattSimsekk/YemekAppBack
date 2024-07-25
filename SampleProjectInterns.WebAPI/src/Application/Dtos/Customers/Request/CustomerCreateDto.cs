using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


using static SampleProjectInterns.Entities.Common.Enums;


namespace Application.Dtos.Customers.Request
{
	public class CustomerCreateDto
	{

		[Required]
		public long company_id { get; set; }
		[Required]
		[MaxLength(512)]

		public string name { get; set; }
		[Required]
		public string surname { get; set; }
		public Gender gender { get; set; }
		public long? phone { get; set; }
		[Required]
		[MaxLength(256)]
		[EmailAddress]
		public string mail { get; set; }
		[Required]
		public string address { get; set; }
		public string? description { get; set; }



	}
}
