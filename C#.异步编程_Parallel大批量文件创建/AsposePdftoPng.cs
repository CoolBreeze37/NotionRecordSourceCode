
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using Aspose.Pdf.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C_.异步编程_Parallel大批量文件创建
{
    public static class AsposePdftoPng
    {
        public static void Convert()
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");

            //文档目录的路径。
            string dataDir = "D:\\Desktop\\PdftoPng\\";
            //打开文档
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Document pdfDocument = new Document(dataDir + "DH_8105D720817_48689464_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE.pdf");
            Parallel.For(1, pdfDocument.Pages.Count, PageNumber =>
            {
                using (FileStream imageStream = new FileStream(dataDir + "\\OutPut\\" + PageNumber + ".png", FileMode.Create))
                {
                    //创建具有指定属性的PNG设备
                    //宽度、高度、分辨率、质量
                    //质量 [0-100]，100 为最大
                    //创建分辨率对象
                    Resolution resolution = new Resolution(600);
                    PngDevice pngDevice = new PngDevice(resolution);
                    //转换特定页面并将图像保存到流中
                    pngDevice.Process(pdfDocument.Pages[PageNumber], imageStream);
                    //关闭流
                    imageStream.Close();
                }
            }
            );
           stopwatch.Stop();
            System.Console.WriteLine($"PDF pages are converted to PNG successfully! Time Cost{stopwatch.ElapsedMilliseconds}");
        }

        public static void JpegConverter()
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
            // Create a new instance of Jpeg
            var converter = new Jpeg();
            // Specify the input and output file paths
            var inputPath = Path.Combine(@"D:\Desktop\PdftoPng", "DH_8105D720817_48689464_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE.pdf");
            var outputPath = Path.Combine(@"D:\Desktop\PdftoPng", "OutPut");

            // Create an instance of the JpegOptions class
            var converterOptions = new JpegOptions();

            // Add the input and output file paths to the options
            converterOptions.AddInput(new FileDataSource(inputPath));
            converterOptions.AddOutput(new FileDataSource(outputPath));
            // Set the output resolution to 300 dpi
            converterOptions.OutputResolution = 600;

            // Set the page range to the first page
            converterOptions.PageList = Enumerable.Range(1, 10000).ToList(); ;
            // Process the conversion and get the result container
            ResultContainer resultContainer = converter.Process(converterOptions);


            // Print the paths of the converted JPEG images
            foreach (FileResult operationResult in resultContainer.ResultCollection.Cast<FileResult>())
            {
                Console.WriteLine(operationResult.Data.ToString());
            }
        }
    }
}
