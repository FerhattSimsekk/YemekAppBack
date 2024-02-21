using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectInterns.Entities
{
    public class Payment:BaseEntity
    {
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public decimal Price { get; set; } 
        public string BillNumber { get; set; } = null!;
        public string? Description { get; set; }      
        public DateTime LastPaymentDate { get; set;}




    }
}
