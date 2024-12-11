using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
	public class KategoriRestoran
	{
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
		public ICollection<Restoran> Restoranlar { get; set; }
		public string? ResimUrl { get; set; }

	}
}
