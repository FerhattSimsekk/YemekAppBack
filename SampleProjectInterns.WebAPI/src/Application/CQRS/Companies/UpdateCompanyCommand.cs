using Application.Dtos.Companies.Request;
using Application.Interfaces.Mailing;
using Application.Interfaces;
using MediatR;
using System.Security.Principal;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Text;
using SampleProjectInterns.Entities;

namespace Application.CQRS.Companies;

public record UpdateCompanyCommand(CompanyUpdateDto Company, long Id) : IRequest;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;
    private readonly IMailSender _mailSender;
    private readonly IStorageProvider _storage;

    public UpdateCompanyCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IMailSender mailSender, IStorageProvider storage)
    {
        _webDbContext = webDbContext;
        _principal = principal;
        _mailSender = mailSender;
        _storage = storage;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");

        var auht = identity.Type;
        if (auht is not AdminAuthorization.admin)
            throw new UnAuthorizedException("Unauthorized access", "Company");

        var company = await _webDbContext.Companies.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
           ?? throw new NotFoundException($"{request.Company.name} not found", "Company");

        company.Address = request.Company.address;
        company.Description = request.Company.description;
        company.Email = request.Company.email;
        company.Host = request.Company.host;
        company.Name = request.Company.name;
        company.PageTitle = request.Company.page_title;
        company.Phone = request.Company.phone.ToString();
        company.ShortName = request.Company.short_name;
        company.Status = request.Company.Status;
        company.TaxAdministration = request.Company.tax_administration;
        company.TaxNumber = request.Company.tax_number;
        company.CityId = request.Company.city_id;
        company.CountyId = request.Company.county_id;
        if (request.Company.logo is not null)
        {
            company.Logo = $"Shared/{company.Id}/{request.Company.logo.FileName}";
            await _storage.Put($"{company.Id}/{request.Company.logo.FileName.Split('.')[0]}.", request.Company?.logo?.OpenReadStream(), request.Company.logo.FileName.Split('.').Last().ToString(), cancellationToken);
        }
        await _webDbContext.SaveChangesAsync(cancellationToken);


        StringBuilder messageBody = new();
        messageBody.Append("<b>Sayın </b>&nbsp; ; " + request.Company.name + " firma bilgilerinizde güncelleme yapılmıştır.<br>");


        await _mailSender.SendMail(
           new Mail()
           {
               Body = new MailBody(MailBodyType.Html) { Text = messageBody.ToString() },
               Subject = "Firma Bilgilerinde Güncelleme",
               To = new List<MailAddress>() { new(request.Company.email, request.Company.email) }
           }
       );
        return Unit.Value;
    }
}
