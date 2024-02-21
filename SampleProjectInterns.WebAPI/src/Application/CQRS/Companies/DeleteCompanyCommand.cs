using Application.Interfaces.Mailing;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Companies
{
    // DeleteCompanyCommand, bir şirketi silmek için kullanılan bir isteği temsil eder.
    public record DeleteCompanyCommand(long Id) : IRequest;

    // DeleteCompanyCommandHandler, DeleteCompanyCommand isteğini işleyen bir sınıftır.
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;
        private readonly IMailSender _mailSender;

        // DeleteCompanyCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public DeleteCompanyCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IMailSender mailSender)
        {
            _webDbContext = webDbContext;
            _principal = principal;
            _mailSender = mailSender;
        }

        // Handle metodu, DeleteCompanyCommand isteğini işler ve Unit yanıtını döndürür.
        public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Kullanıcının yetkilendirme seviyesi kontrol edilir, admin olmayanlar yetkilendirme hatası alır.
            var auht = identity.Type;
            if (auht is not AdminAuthorization.admin)
                throw new UnAuthorizedException("Unauthorized access", "Company");

            // Silinecek şirket veritabanından alınır, bulunamazsa hata alınır.
            var company = await _webDbContext.Companies.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Company not found", "Company");

            // Şirketin durumu "silinmiş" olarak güncellenir ve veritabanı kaydedilir.
            company.Status = Status.deleted;
            await _webDbContext.SaveChangesAsync(cancellationToken);

            // Silinen şirket için bir bilgilendirme e-postası oluşturulur ve gönderilir.
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
}
