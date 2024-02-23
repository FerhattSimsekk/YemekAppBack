using Application.CQRS.Companies; // Uygulama katmanındaki Companies namespace'i
using Application.CQRS.Employees; // Uygulama katmanındaki Employees namespace'i
using Application.Dtos.Employees.Request; // Employees namespace'indeki istek DTO'ları
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.AccessControl;

namespace Presentation.Controllers // Presentation katmanındaki Controllers namespace'i
{
    [Route("api/[controller]")] // API endpoint'i belirleme
    [ApiController] // Controller'ın bir Web API controller'ı olduğunu belirtme
    [Authorize(Policy = "verified", Roles = "admin")]



    public class EmployeeController: ControllerBase // ControllerBase sınıfından türetilen EmployeeController sınıfı
    {
        private readonly ISender _sender; // MediatR kütüphanesindeki ISender arayüzü kullanılarak işlem gönderme

        public EmployeeController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sender.Send(new GetEmployeeQuery()));
        }


        [HttpPost] // HTTP POST isteğine yanıt verme
        public async Task<IActionResult> Post([FromBody] EmployeeCreateDto employee) // EmployeeCreateDto tipinden bir nesne alarak yeni bir çalışan oluşturma
        {
            return Ok(await _sender.Send(new CreateEmployeeCommand(employee))); // CreateEmployeeCommand kullanarak işlemi yürütme ve başarılı yanıt döndürme
        }






        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id,EmployeeUpdateDto employee)
        {
            return Ok(await _sender.Send(new UpdateEmployeeCommand(employee,id)));
        }





        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _sender.Send(new GetEmployeeByIdQuery(id)));
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeCommmnad (long id)
        {
            return Ok(await _sender.Send(new DeleteEmployeeCommand(id)));
        }


        [HttpGet("PdfQuery")]
        public async Task<IActionResult> GetEmployeePdfQuery()
        {
            var DataResult = await _sender.Send(new GetEmployeePdfQuery());
            return File(DataResult, MediaTypeNames.Application.Pdf , "employe-list.pdf");

        }


    }
}
