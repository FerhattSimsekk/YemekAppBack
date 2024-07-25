﻿using Application.Dtos.Customers.Response;
using Application.Dtos.Payments.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Payments
{
	public record GetPaymentsQuery() : IRequest<List<PaymentDto>>;

	public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, List<PaymentDto>>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public GetPaymentsQueryHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<List<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Payment");

			var payments = await _webDbContext.Payments
				.AsNoTracking()
				.OrderByDescending(order => order.CreatedAt)
				.Select(customers => customers.MapToPaymentDto())
				.ToListAsync(cancellationToken);

			return payments;
		}
	}
}
