using Application.Dtos.Identities.Request;
using Application.Dtos.Identities.Response;
using Application.Interfaces;
using Application.Interfaces.Mailing;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;
using SampleProjectInterns.Entities;
using System.Security.Principal;
using System.Text;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Identities;
public record CreateIdentityCommand(IdentityCreateDto Identity) : IRequest<IdentityDto>;
public class CreateIdentityCommandHandler : IRequestHandler<CreateIdentityCommand, IdentityDto>
{
    private readonly IWebDbContext _webDbContext;
    private readonly IPrincipal _principal;
    private readonly IMailSender _mailSender;
    public CreateIdentityCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IMailSender mailSender)
    {
        _webDbContext = webDbContext;
        _principal = principal;
        _mailSender = mailSender;
    }

    public async Task<IdentityDto> Handle(CreateIdentityCommand request, CancellationToken cancellationToken)
    {
        



        //var identity = await _webDbContext.Identities.AsNoTracking()
        //  .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
        //  ?? throw new Exception("User not found");

        //bool isUserEmailInUse = await _webDbContext.Identities
        //   .AnyAsync(identity => identity.Email == request.Identity.Email, cancellationToken);

        //if (isUserEmailInUse)
        //    throw new Domain.Exceptions.ValidationException("Bu email adresi zaten kullanılıyor.", nameof(IdentityCreateDto.Email));

        var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: false, passwordLength: 8);

        var password = "232323";// pwd.Next();
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        var newIdentity = new Identity()
        {
            RestoranId = request.Identity.RestoranId == 0 ? null : request.Identity.RestoranId,// request.Identity.
                                                                                               //
                                                                                               // Id,
            Email = request.Identity.Email,
            Password = hashedPassword,
            Salt = salt,
            Type = (AdminAuthorization)request.Identity.Type,//  (identity.Type == AdminAuthorization.admin || identity.Type == AdminAuthorization.moderator) ? (AdminAuthorization)request.Identity.Type : AdminAuthorization.user,
            Name = request.Identity.Name,
            LastName = request.Identity.Surname,
            Status = Status.approved
        };

        await _webDbContext.Identities.AddAsync(newIdentity, cancellationToken);
        await _webDbContext.SaveChangesAsync(cancellationToken);
        StringBuilder messageBody = new();
        messageBody.Append("Kullanıcı hesabınız oluşturulmuştur. Sisteme giriş yapabilmeniz için bilgileriniz aşağıda yer almaktadır.<br>");
        messageBody.Append("<b>E-Posta</b>&nbsp;: " + request.Identity.Email + "<br>");
        messageBody.Append("<b>Şifre</b>&nbsp;: " + password + "<br>");

        await _mailSender.SendMail(
           new Mail()
           {
               Body = new MailBody(MailBodyType.Html) { Text = messageBody.ToString() },
               Subject = "Hesap Oluşturulması & Yeni Şifre",
               To = new List<MailAddress>() { new(request.Identity.Email, request.Identity.Email) }
           }
       );
        return newIdentity.MapToIdentityDto();
    }
}
