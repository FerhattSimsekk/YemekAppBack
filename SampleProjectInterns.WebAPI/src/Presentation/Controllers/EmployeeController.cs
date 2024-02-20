using Application.CQRS.Companies; // Uygulama katmanındaki Companies namespace'i
using Application.CQRS.Employees; // Uygulama katmanındaki Employees namespace'i
using Application.Dtos.Employees.Request; // Employees namespace'indeki istek DTO'ları
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers // Presentation katmanındaki Controllers namespace'i
{
    [Route("api/[controller]")] // API endpoint'i belirleme
    [ApiController] // Controller'ın bir Web API controller'ı olduğunu belirtme



    public class EmployeeController: ControllerBase // ControllerBase sınıfından türetilen EmployeeController sınıfı
    {
        private readonly ISender _sender; // MediatR kütüphanesindeki ISender arayüzü kullanılarak işlem gönderme

        public EmployeeController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost] // HTTP POST isteğine yanıt verme
        public async Task<IActionResult> Post([FromBody] EmployeeCreateDto employee) // EmployeeCreateDto tipinden bir nesne alarak yeni bir çalışan oluşturma
        {
            return Ok(await _sender.Send(new CreateEmployeeCommand(employee))); // CreateEmployeeCommand kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }
    }
}
