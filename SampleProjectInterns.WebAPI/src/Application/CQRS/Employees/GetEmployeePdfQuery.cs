using Application.Dtos.Employees.Response;          // Bu kısım, Employees için Response tipinde DTO'ların bulunduğu namespace'i ekler.
using Application.Interfaces;                       // Uygulamanın kullanacağı arayüzleri içeren namespace.
using Application.Mappers;                          // DTO'ları domain modellerine ve tam tersine dönüştüren sınıfları içeren namespace.
using AutoMapper;                                   // Obje haritalamalarını (mapping) sağlayan AutoMapper kütüphanesinin kullanıldığı namespace.
using iTextSharp.text;                              // PDF oluşturmak için iTextSharp kütüphanesinin kullanıldığı namespace.
using iTextSharp.text.pdf;                          // PDF oluşturma işlemlerini içeren namespace.
using MediatR;                                      // Mediator pattern'ini sağlayan MediatR kütüphanesinin kullanıldığı namespace.
using Microsoft.EntityFrameworkCore;                // Entity Framework Core ile ilgili namespace.
using System.Security.Principal;                    // Windows kimlik doğrulamasıyla ilgili namespace.

namespace Application.CQRS.Employees
{
    public record GetEmployeePdfQuery() : IRequest<byte[]>; // Employee'leri PDF formatında almak için kullanılan sorgu tipi.

    public class GetEmployeePdfQueryHandler : IRequestHandler<GetEmployeePdfQuery, byte[]>
    {
        private readonly IWebDbContext _webDbContext;   // Web uygulaması için DbContext arayüzü.
        private readonly IPrincipal _principal;         // Kullanıcının kimliğini temsil eden arayüz.
        private readonly IMapper _mapper;               // Obje haritalamalarını yapan arayüz.

        public GetEmployeePdfQueryHandler(IWebDbContext webDbContext, IPrincipal principal, IMapper mapper)
        {
            _webDbContext = webDbContext;   // Bağımlılıkları enjekte et.
            _principal = principal;         // Bağımlılıkları enjekte et.
            _mapper = mapper;               // Bağımlılıkları enjekte et.
        }

        public async Task<byte[]> Handle(GetEmployeePdfQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının kimliğini al ve eşleşen bir kimlik bulamazsan hata fırlat.
            var identity = await _webDbContext.Identities.AsNoTracking()
                .FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
                ?? throw new Exception("User not found");

            // Tüm employee'leri oluşturulma tarihine göre sırala ve DTO'ya dönüştürerek listele.
            var employees = await _webDbContext.Employees.AsNoTracking()
                 .OrderByDescending(order => order.CreatedAt)
                 .Select(employee => employee.MapToEmployeeDto())
                 .ToListAsync(cancellationToken);

            // PDF dosyasını oluştur ve byte dizisini döndür.
            var pdfBytes = EmpoloyePdfCreated(employees);
            return pdfBytes;
        }

        private byte[] EmpoloyePdfCreated(List<EmployeeDto> employeeDtos)
        {
            // PDF dosyasının başlık ve detay kolon genişlikleri.
            var headerWidthsTitle = new[] { 200f };
            var headerWidthsMountly = new[] { 20f, 60f, 100f, 60f, 60f, 70f, 30f, 70f, 100f };

            // PDF belgesini oluştur.
            var pdfDocument = new Document(new Rectangle(288f, 144f), -70f, -70f, 30f, 30f);
            pdfDocument.SetPageSize(PageSize.A4.Rotate());

            // Bellek akışı oluştur.
            var ms = new MemoryStream();

            // Türkçe karakterleri destekleyen yazı fontunu belirle.
            var STF_Helvetica_Turkish = iTextSharp.text.pdf.BaseFont.CreateFont("Helvetica", "Cp1254", iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
            var turkishFont = new iTextSharp.text.Font(STF_Helvetica_Turkish, 10, iTextSharp.text.Font.NORMAL);

            // Sayfa olaylarını (header/footer) yöneten sınıfı oluştur.
            _events e = new _events();

            // PDF yazarını oluştur ve sayfa olaylarını ekle.
            PdfWriter pw = PdfWriter.GetInstance(pdfDocument, ms);
            pw.PageEvent = e;

            // PDF belgesini aç.
            pdfDocument.Open();

            // PDF başlık bilgilerini ekle.
            var headerTableImageTable = new PdfPTable(headerWidthsTitle) { SpacingAfter = 0, SpacingBefore = 10 };
            pdfDocument.AddHeader("Header", "Personel Listesi");

            // Başlık için paragraf ve metin oluştur.
            Paragraph p = new Paragraph();
            Chunk c1 = new Chunk($"Personel Listesi", new iTextSharp.text.Font(STF_Helvetica_Turkish, 11, iTextSharp.text.Font.BOLD));
            p.Add(c1);
            p.IndentationLeft = 100;
            pdfDocument.Add(p);

            // PDF'e detay başlıklarını ekle.
            p = new Paragraph();
            c1 = new Chunk("Personel Listesi", new iTextSharp.text.Font(STF_Helvetica_Turkish, 10, iTextSharp.text.Font.NORMAL));
            p.Add(c1);
            p.IndentationLeft = 100;
            pdfDocument.Add(p);

            // Detay tablosunu oluştur.
            var headerTableDetayTable = new PdfPTable(headerWidthsMountly) { SpacingAfter = 1, SpacingBefore = 10 };
            headerTableDetayTable.TotalWidth = 770F;
            headerTableDetayTable.WriteSelectedRows(10, -1, 220, (pdfDocument.PageSize.Height - 100), pw.DirectContent);
            var colorHeader = new iTextSharp.text.BaseColor(211, 211, 211);

            // Detay başlıklarını oluştur.
            var headerCellsDetay = new[]
            {
                new PdfPCell(new Phrase("Sıra No", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Adı Soyadı", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Mail", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Telefon No -1", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Telefon No -2" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Departman" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Cinsiyet" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Adres" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Açıklama" , turkishFont)) {HorizontalAlignment = 1, VerticalAlignment=Element.ALIGN_MIDDLE,BackgroundColor = colorHeader, BorderWidth = 1},
            };

            // Detay başlıklarını tabloya ekle.
            foreach (var pdfPCell in headerCellsDetay)
            {
                headerTableDetayTable.AddCell(pdfPCell);
            }

            // Detay tablosunu PDF'e ekle.
            pdfDocument.Add(headerTableDetayTable);

            // Detay verilerini işle.
            var headerTableDetay = new PdfPTable(headerWidthsMountly) { SpacingAfter = 0, SpacingBefore = 0 };
            int index = 1;
            foreach (var employee in employeeDtos)
            {
                var colorBorder = new iTextSharp.text.BaseColor(221, 221, 221);
                var color = new iTextSharp.text.BaseColor(255, 255, 255);

                var headerTableDetayCells = new[]
                {
                    new PdfPCell(new Phrase(index.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.name.ToString()+" "+employee.surname.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.mail.ToString(), turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.phone.ToString(), turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.phone2.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.department, turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.gender.ToString(), turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.address, turkishFont)) {HorizontalAlignment = 0, VerticalAlignment=Element.ALIGN_MIDDLE,BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(employee.description, turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder}
                };

                foreach (var pdfPCell in headerTableDetayCells)
                {
                    headerTableDetay.AddCell(pdfPCell);
                }
                index++;
            }

            // Detay tablosunu PDF'e ekle.
            pdfDocument.Add(headerTableDetay);

            // PDF belgesini kapat ve bellek akışını byte dizisine dönüştürerek döndür.
            pdfDocument.Close();
            return ms.ToArray();
        }
    }

    // Sayfa olaylarını yöneten sınıf. 2. sayfaya geçemesi durumunda başlıklar olur.
    internal class _events : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            if (document.PageNumber == 1)
            {
                return;
            }

            // Türkçe karakterleri destekleyen yazı fontunu belirle.
            var STF_Helvetica_Turkish = iTextSharp.text.pdf.BaseFont.CreateFont("Helvetica", "Cp1254", iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
            var turkishFont = new iTextSharp.text.Font(STF_Helvetica_Turkish, 10, iTextSharp.text.Font.NORMAL);

            // PDF başlık bilgilerini oluştur.
            var headerWidthsTahliller = new[] { 20f, 60f, 100f, 60f, 60f, 70f, 30f, 70f, 100f };
            var headerTableDetayTable = new PdfPTable(headerWidthsTahliller) { SpacingAfter = 1, SpacingBefore = 10, TotalWidth = 500F };
            var colorHeader = new iTextSharp.text.BaseColor(211, 211, 211);

            // Detay başlıklarını oluştur.
            var headerCellsDetay = new[]
            {
                new PdfPCell(new Phrase("Sıra No", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Adı Soyadı", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Mail", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Telefon No -1", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Telefon No -2" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Departman" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Cinsiyet" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Adres" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Açıklama" , turkishFont)) {HorizontalAlignment = 1, VerticalAlignment=Element.ALIGN_MIDDLE,BackgroundColor = colorHeader, BorderWidth = 1},
            };

            // Detay başlıklarını tabloya ekle.
            foreach (var pdfPCell in headerCellsDetay)
            {
                headerTableDetayTable.AddCell(pdfPCell);
            }

            // Detay tablosunu PDF'e ekle.
            document.Add(headerTableDetayTable);
        }
    }
}
