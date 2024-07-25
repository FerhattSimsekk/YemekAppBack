using Application.Dtos.Payments.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class PaymentMapper
	{
		public static PaymentDto MapToPaymentDto(this Payment payment)
		{
			return new PaymentDto(
			   payment.Id,
			   payment.CompanyId,
			   payment.CustomerId,
			   payment.Price,
			   payment.BillNumber,
			   payment.Description,
			   payment.LastPaymentDate,
			   payment.CreatedAt,
			   payment.UpdatedAt,
			   payment.Status

			);
		}
	}
}