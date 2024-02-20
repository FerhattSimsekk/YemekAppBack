using Application.CQRS.Companies; // Uygulama katmanındaki Companies namespace'i
using Application.CQRS.Customers; // Uygulama katmanındaki Customers namespace'i
using Application.CQRS.Employees; // Uygulama katmanındaki Employees namespace'i
using Application.Dtos.Employees.Request; // Employees namespace'indeki istek DTO'ları
using MediatR; // MediatR kütüphanesi
using Microsoft.AspNetCore.Http; // ASP.NET Core HTTP kütüphanesi
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC kütüphanesi
using NuGet.Protocol.Plugins; // NuGet Protocol Plugins kütüphanesi

namespace Presentation.Controllers // Presentation katmanındaki Controllers namespace'i
{
    [Route("api/[controller]")] // API endpoint'i belirleme
    [ApiController] // Controller'ın bir Web API controller'ı olduğunu belirtme



    public class EmployeeController : ControllerBase // ControllerBase sınıfından türetilen EmployeeController sınıfı
    {
        private readonly MediatR.ISender _sender; // MediatR kütüphanesindeki ISender arayüzü kullanılarak işlem gönderme



        public EmployeeController(MediatR.ISender sender) // Constructor Dependency Injection kullanarak ISender enjekte etme
        {
            _sender = sender;
        }



        [HttpPost] // HTTP POST isteğine yanıt verme
        public async Task<IActionResult> Post([FromBody] EmployeeCreateDto employee) // EmployeeCreateDto tipinden bir nesne alarak yeni bir çalışan oluşturma
        {
            return Ok(await _sender.Send(new CreateEmployeeCommand(employee))); // CreateEmployeeCommand kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }






        [HttpGet("{id}")] // HTTP GET isteğine yanıt verme, id parametresi ile çalışanı getirme
        public async Task<IActionResult> Get(long id) // id parametresini alarak çalışanı getirme
        {
            if (id == null) // id null ise NotFound döndürme
                return NotFound();

            return Ok(await _sender.Send(new GetCompanyByIdQuery(id))); // GetCompanyByIdQuery kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }






        [HttpPut("{id}")] // HTTP PUT isteğine yanıt verme, id parametresi ile çalışanı güncelleme
        public async Task<IActionResult> Put(long id, EmployeeUpdateDto employee) // id parametresini ve güncellenecek çalışanı alarak güncelleme işlemi
        {
            if (id==null) // id null ise NotFound döndürme
                return NotFound();

            return Ok(await _sender.Send(new UpdateEmployeeCommand(employee, id))); // UpdateEmployeeCommand kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }






        [HttpDelete("{id}")] // HTTP DELETE isteğine yanıt verme, id parametresi ile çalışanı silme
        public async Task<IActionResult> Delete(long id) // id parametresini alarak çalışanı silme
        {
            if (id == null) // id null ise NotFound döndürme
                return NotFound();

            await _sender.Send(new DeleteEmployeeCommand(id)); // DeleteEmployeeCommand kullanarak işlemi yürütme
            return Ok(); // Başarılı yanıt döndürme
        }






        [HttpGet] // HTTP GET isteğine yanıt verme, tüm çalışanları getirme
        public async Task<IActionResult> Get()
        {
            return Ok(await _sender.Send(new GetCompaniesQuery())); // GetCompaniesQuery kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }
    }
}
