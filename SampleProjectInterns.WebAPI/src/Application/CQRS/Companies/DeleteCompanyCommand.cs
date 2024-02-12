using Application.Interfaces.Mailing;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Companies; 

public record DeleteCompanyCommand(long Id) : IRequest;
public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;
    private readonly IMailSender _mailSender;

    public DeleteCompanyCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IMailSender mailSender)
    {
        _webDbContext = webDbContext;
        _principal = principal;
        _mailSender = mailSender;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var identity = await _webDbContext.Identities.AsNoTracking()
         .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
         ?? throw new Exception("User not found");

        var auht = identity.Type;
        if (auht is not AdminAuthorization.admin)
            throw new UnAuthorizedException("Unauthorized access", "Company");

        var company = await _webDbContext.Companies.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
           ?? throw new NotFoundException($"Company not found", "Company");


        company.Status = Status.deleted;
        await _webDbContext.SaveChangesAsync(cancellationToken);


        StringBuilder messageBody = new();
        messageBody.Append("<b>Sayın </b>&nbsp; ; " + company.Name + " firma hesabınız silinmiştir.<br>");


        await _mailSender.SendMail(
           new Mail()
           {
               Body = new MailBody(MailBodyType.Html) { Text = messageBody.ToString() },
               Subject = "Firma Hesabı Silme",
               To = new List<MailAddress>() { new(company.Email, company.Email) }
           }
       );
        return Unit.Value;
    }
}