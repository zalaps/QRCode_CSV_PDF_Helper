using CsvHelper;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace QRApp
{
    public class QRHelper
    {
        public void CreateSampleStructure(string path)
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Console.WriteLine("The directory QRApp already exists.");
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", path);
            }

            var recordsForSample = new List<Foo>
                {
                    new Foo { Filename = "1", Code = "one" },
                    new Foo { Filename = "2", Code = "two" }
                };

            using (var writer = new StreamWriter("D:/QRApp/codes.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(recordsForSample);
            }

            Console.WriteLine("Sample file codes.csv saved successfully at {0}.", path);
        }

        public List<Foo> GenerateQRCodes(string path)
        {
            var records = new List<Foo>();
            var i = 0;
            using (var reader = new StreamReader("D:/QRApp/codes.csv"))
            using (var csv = new CsvReader(reader))
            {
                records = csv.GetRecords<Foo>().ToList();

                foreach (var item in records)
                {
                    i++;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.Code, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    qrCodeImage.Save("D:/QRApp/" + item.Filename + ".png", ImageFormat.Png);
                    Console.Write("\r{0} QR Codes generated successfully at at {1}", i, path);
                }
            }
            Console.WriteLine(".");
            return records;
        }
    }
}
