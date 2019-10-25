using System;
using System.Collections.Generic;
using System.IO;

namespace QRApp
{
    class Program
    {
        static string Path { get; set; }
        static string PDFPath { get; set; }       

        static void Main(string[] args)
        {            
            try
            {
                var csvrecords = new List<Foo>();
                var qrhelper = new QRHelper();

                // Specify the directory you want to manipulate.
                Path = @"D:\QRApp";
                PDFPath = @"D:\QRApp\output.pdf";

                Console.WriteLine("This is a utility to generate QR Codes in bulk.");
                Console.WriteLine("Use following inputs as per requirement:");
                Console.WriteLine("Enter 1 # Generates sample file structure at path D:/QRApp. Use this option for very first time.");
                Console.WriteLine("Enter 2 # Generates QR Codes in bulk. Use file codes.csv to provide data to generate QR Codes.");

                var c = Console.ReadKey().KeyChar;

                // QR Play
                switch (c)
                {
                    case '1':
                        qrhelper.CreateSampleStructure(Path);
                        csvrecords = qrhelper.GenerateQRCodes(Path);
                        break;
                    case '2':
                        csvrecords = qrhelper.GenerateQRCodes(Path);
                        break;
                    default:
                        Console.WriteLine("Please enter either 1 or 2.");
                        break;
                }

                // PDF Play
                FileInfo file = new FileInfo(Path + "\\output.pdf");
                file.Directory.Create();
                new PDFHelper().CreatePdf(Path, csvrecords);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally
            {
                Console.WriteLine("Enjoyed? You owe a cigarette to prady!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }   
}
