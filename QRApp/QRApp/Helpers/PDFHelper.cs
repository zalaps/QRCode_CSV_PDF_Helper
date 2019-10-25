using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;

namespace QRApp
{
    public class PDFHelper
    {
        public virtual void CreatePdf(string dest, List<Foo> records)
        {
            //Record Counter
            var i = 1;
            var j = 1;

            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest + "\\output.pdf");
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);
            document.SetMargins(10, 10, 10, 16);

            //Add table to the document
            Console.WriteLine("Please enter desired height of the QR Code. Please keep value in beetween 10-100");
            var cellimagewidth = Convert.ToInt32(Console.ReadLine());
            int celltextwidth = 144;

            // Total width of page is considered as 520 points. 
            var columnWidths = UnitValue.CreatePointArray(new float[] {
                (celltextwidth - cellimagewidth),
                cellimagewidth,
                (celltextwidth - cellimagewidth),
                cellimagewidth,
                (celltextwidth - cellimagewidth),
                cellimagewidth,
                (celltextwidth - cellimagewidth),
                cellimagewidth });
            var table = new Table(columnWidths, true).SetWidth(new UnitValue(1, 520));

            foreach (var part in records)
            {                
                var imagepath = dest + "\\" + part.Filename + ".png";
                var qrcode = new Image(ImageDataFactory.Create(imagepath));
                
                // To break page after 44 codes
                if (j == 45)
                {
                    document.Add(table);
                    document.Add(new AreaBreak());
                    table = null;
                    j = 1;
                    table = new Table(columnWidths, true).SetWidth(new UnitValue(1, 520));                                       
                }                    

                table.AddCell(new Cell()
                    .Add(new Paragraph(part.Code))
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(8)
                    .SetPadding(10)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER));

                table.AddCell(new Cell()
                    .Add(qrcode.SetAutoScale(true))
                    .SetMaxHeight(cellimagewidth)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(2)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER));

                Console.Write("\r{0} QR Codes placed in pdf file at {1}", i, dest + "\\output.pdf");
                i++;
                j++;
            }

            document.Add(table);
            Console.WriteLine(".");

            //Close document
            document.Close();
        }
    }
}
