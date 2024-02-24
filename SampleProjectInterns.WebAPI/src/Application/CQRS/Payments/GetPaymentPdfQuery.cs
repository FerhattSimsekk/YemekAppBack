
using Application.Dtos.Payments.Response;
using Application.Interfaces;
using Application.Mappers;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

using AutoMapper;

namespace Application.CQRS.Payments
{
    public record GetPaymentPdfQuery():IRequest<byte[]>;

    public class GetPaymentPdfQueryHandler : IRequestHandler<GetPaymentPdfQuery, byte[]>

    {
        private readonly IWebDbContext _webDbContext;
        private readonly IPrincipal _principal;
        private readonly IMapper _mapper;

        public GetPaymentPdfQueryHandler(IWebDbContext webDbContext, IPrincipal principal, IMapper mapper)
        {
            _webDbContext = webDbContext;
            _principal = principal;
            _mapper = mapper;
        }

        public async  Task<byte[]> Handle(GetPaymentPdfQuery request, CancellationToken cancellationToken)
        {
            var identity = await _webDbContext.Identities.AsNoTracking().
            FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken) ??
            throw new Exception("User Not Found");

            var payments = await _webDbContext.Payments.AsNoTracking().OrderByDescending(order => order.CreatedAt)
            .Select(payment => payment.MapToPaymentDto()).ToListAsync(cancellationToken);

            var pdfbytes = PaymentPdfCreated(payments);
            return pdfbytes;
        }

       
        private byte[] PaymentPdfCreated(List<PaymentDto> paymentDtos) 
        {
            // PDF dosyasının başlık ve detay kolon genişlikleri.
            var headerWidthsTitle = new[] { 200f };
            var headerWidthsMountly = new[] { 20f, 60f, 100f, 60f, 60f };

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
            pdfDocument.AddHeader("Header", "Ödeme Listesi");

            // Başlık için paragraf ve metin oluştur.
            Paragraph p = new Paragraph();
            Chunk c1 = new Chunk($"Ödeme Listesi", new iTextSharp.text.Font(STF_Helvetica_Turkish, 11, iTextSharp.text.Font.BOLD));
            p.Add(c1);
            p.IndentationLeft = 100;
            pdfDocument.Add(p);

            // PDF'e detay başlıklarını ekle.
            p = new Paragraph();
            c1 = new Chunk("Ödeme Listesi", new iTextSharp.text.Font(STF_Helvetica_Turkish, 10, iTextSharp.text.Font.NORMAL));
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
                new PdfPCell(new Phrase("Ödeme Tutarı", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Fatura No", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Acıklama", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                new PdfPCell(new Phrase("Tarih" , turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
                
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
            foreach (var payment in paymentDtos)
            {
                var colorBorder = new iTextSharp.text.BaseColor(221, 221, 221);
                var color = new iTextSharp.text.BaseColor(255, 255, 255);

                var headerTableDetayCells = new[]
                {
                    new PdfPCell(new Phrase(index.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(payment.price.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(payment.bill_number.ToString(), turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(payment.description, turkishFont)) {HorizontalAlignment = 0,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                    new PdfPCell(new Phrase(payment.last_payment_date.ToString(), turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = color, BorderColor = colorBorder},
                                      
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
            var headerWidthsTahliller = new[] { 20f, 60f, 100f, 60f, 60f };
            var headerTableDetayTable = new PdfPTable(headerWidthsTahliller) { SpacingAfter = 1, SpacingBefore = 10, TotalWidth = 500F };
            var colorHeader = new iTextSharp.text.BaseColor(211, 211, 211);

            // Detay başlıklarını oluştur.
            var headerCellsDetay = new[]
            {
            new PdfPCell(new Phrase("Sıra No", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
            new PdfPCell(new Phrase("Ödeme Tutarı", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
            new PdfPCell(new Phrase("Fatura No", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
            new PdfPCell(new Phrase("Acıklama ", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
            new PdfPCell(new Phrase("Tarih ", turkishFont)) {HorizontalAlignment = 1,VerticalAlignment=Element.ALIGN_MIDDLE, BackgroundColor = colorHeader, BorderWidth = 1},
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
