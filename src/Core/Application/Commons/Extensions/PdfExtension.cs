using iText.Html2pdf;
using System.IO;

namespace Application.Commons.Extensions
{
    public static class PdfExtension
    {
        public static byte[] PrintPdf(this string html)
        {
            using (var stream = new MemoryStream())
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(html, stream);
                byte[] pdfBuffer = stream.ToArray();
                return pdfBuffer;
            }
        }
    }
}