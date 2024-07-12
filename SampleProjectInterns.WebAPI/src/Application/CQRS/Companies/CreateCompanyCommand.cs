using Application.Dtos.Companies.Request;
using Application.Dtos.Companies.Response;
using Application.Interfaces;
using Application.Interfaces.Mailing;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;
using System.Text;

namespace Application.CQRS.Companies
{
    // CreateCompanyCommand, yeni bir şirket oluşturmak için kullanılan bir isteği temsil eder.
    public record CreateCompanyCommand(CompanyCreateDto Company) : IRequest<CompanyDto>;

    // IRequestHandler'dan kalıtım alarak CreateCompanyCommand'ı işleyen bir sınıf oluşturuldu.
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;
        private readonly IMailSender _mailSender;
        private readonly IStorageProvider _storage;
 

        // CreateCompanyCommandHandler, gerekli bağımlılıkları alarak oluşturulur.
        public CreateCompanyCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IMailSender mailSender, IStorageProvider storage)
        {
            _webDbContext = webDbContext;
            _principal = principal;
            _mailSender = mailSender;
            _storage = storage;
        }

        // Handle metodu, CreateCompanyCommand isteğini işler ve CompanyDto yanıtını döndürür.
        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliği alınır.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Kullanıcının yetkilendirme seviyesi kontrol edilir, admin olmayanlar yetkilendirme hatası alır.
            var auht = identity.Type;
            if (auht is not SampleProjectInterns.Entities.Common.Enums.AdminAuthorization.admin)
                throw new UnAuthorizedException("Unauthorized access", "Company");

            // Yeni bir Company nesnesi oluşturulur ve request.Company bilgileri ile doldurulur.
            Company company = new()
            {
                Address = request.Company.address,
                Description = request.Company.description,
                Email = request.Company.email,
                Host = request.Company.host,
                Logo = request.Company.logo?.FileName,
                Name = request.Company.name,
                PageTitle = request.Company.page_title,
                Phone = request.Company.phone.ToString(),
                ShortName = request.Company.short_name,
                Status = SampleProjectInterns.Entities.Common.Enums.Status.approved,
                TaxAdministration = request.Company.tax_administration,
                TaxNumber = request.Company.tax_number,
                CityId = request.Company.city_id,
                CountyId = request.Company.county_id
            };

            // Şirket veritabanına eklenir.
            await _webDbContext.Companies.AddAsync(company, cancellationToken);
            await _webDbContext.SaveChangesAsync(cancellationToken);

            // Şirketin logo bilgisi mevcutsa, depolanır ve veritabanı güncellenir.
            if (request.Company.logo is not null)
            {
                await _storage.Put($"{company.Id}/{request.Company.logo.FileName.Split('.')[0]}.", request.Company?.logo?.OpenReadStream(), request.Company.logo.FileName.Split('.').Last().ToString(), cancellationToken);
                company.Logo = $"Shared/{company.Id}/{request.Company.logo.FileName}";
                await _webDbContext.SaveChangesAsync(cancellationToken);
            }

            // Yeni şirket için bir hoş geldin e-postası oluşturulur ve gönderilir.
            StringBuilder messageBody = new();
            messageBody.Append("<b>Sayın </b>&nbsp;; " + request.Company.name + " firmanızın kaydı tamamlanmıştır. Kullanıcı bilgileriniz en kısa zamanda sizinle paylaşılacaktır.<br>");
            messageBody.Append("<b>Hayırlı olsun.</b>");

            await _mailSender.SendMail(
                new Mail()
                {
                    Body = new MailBody(MailBodyType.Html) { Text = messageBody.ToString() },
                    Subject = "Yeni Kayıt İşlemi",
                    To = new List<MailAddress>() { new(request.Company.email, request.Company.email) }
                }
            );

            // Oluşturulan şirketin şehir ve ilçe bilgileri alınır.
            var cities = await _webDbContext.Cities.AsNoTracking().FirstOrDefaultAsync(city => city.Key == company.CityId, cancellationToken);
            var counties = await _webDbContext.Counties.AsNoTracking().FirstOrDefaultAsync(county => county.Key == company.CountyId, cancellationToken);

            // Oluşturulan şirket CompanyDto'ya dönüştürülerek döndürülür.
            return company.MapToCompanyDto(cities?.Name, counties?.Name);
        }
    }
}
