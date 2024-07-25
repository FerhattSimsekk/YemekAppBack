using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities
{
	public class Customer:BaseEntity
	{
		public long CompanyId { get; set; }
		public string Name { get; set; } = null!;
		public string Surname { get; set; } = null!;
		public Gender Gender { get; set; }
		public long? Phone { get; set; }
		public string Mail { get; set; } = null!;
		public string Address { get; set; } = null!;
		public string? Description { get; set; }
		public ICollection<Payment> Payments { get; set; } = new List<Payment>();

	}
}
