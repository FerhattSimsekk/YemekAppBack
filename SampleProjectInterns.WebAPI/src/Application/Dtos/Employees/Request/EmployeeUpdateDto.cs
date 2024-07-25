using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;
using System.ComponentModel.DataAnnotations;


namespace Application.Dtos.Employees.Request
{
	public class EmployeeUpdateDto
	{
		[Required]
		public long company_id { get; set; }
		[Required]
		[MaxLength(512)]
		public string name { get; set; }
		[Required]
		public string surname { get; set; }

		public Gender gender { get; set; }
		[Required]
		public long phone { get; set; }
		[Required,EmailAddress,MaxLength(512)]
		public string mail { get; set; }
		[Required]
		public string address { get; set; }
		public string department { get; set; }
		public string? description { get; set; }

		public Status status { get; set; }
	}
}
