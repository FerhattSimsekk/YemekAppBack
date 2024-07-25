using Application.Dtos.Customers.Request;
using Application.Interfaces;
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

namespace Application.CQRS.Customers
{
	public record UpdateCustomerCommand(CustomerUpdateDto Customer, long CostumerId) : IRequest;
	public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public UpdateCustomerCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
		{

			var identity = await _webDbContext.Identities.AsNoTracking()
			.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
			?? throw new Exception("User not found");
			var auht = identity.Type;
			if (auht is not AdminAuthorization.admin)
				throw new UnAuthorizedException("Unauthorized access", "Customer");

			var customer = await _webDbContext.Customers.FirstOrDefaultAsync(i => i.Id == request.CostumerId, cancellationToken)
				?? throw new NotFoundException($"{request.Customer.name} not found", "Customer");

			
			customer.Address = request.Customer.address;
			customer.Description = request.Customer.description;
			customer.Mail = request.Customer.mail;
			customer.Name = request.Customer.name;
			customer.Surname = request.Customer.surname;
			customer.Gender = request.Customer.gender;
			customer.Phone = request.Customer.phone;
			customer.Status = request.Customer.status;
			customer.UpdatedAt= DateTime.Now;

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

		}
	}
}
