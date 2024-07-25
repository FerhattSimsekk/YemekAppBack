using Application.Dtos.Customers.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class CustomerMapper
	{
		public static CustomerDto MapToCustomerDto(this Customer customer)
		{
			return new CustomerDto(
				customer.Id,
				customer.CompanyId,
				customer.Name,
				customer.Surname,
				customer.Gender,
				customer.Phone,
				customer.Mail,
				customer.Address,
				customer.Description,
				customer.CreatedAt,
				customer.UpdatedAt,
				customer.Status
				);

		}
	}
}
