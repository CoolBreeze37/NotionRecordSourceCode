using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace C_.异步编程_Parallel大批量文件创建
{
    public static class SplitPdf
    {

        public static bool Split(string inputFile, string outputPath, int startPage, int endPage)
        {
            try
            {
                new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");

                PdfFileEditor pdfFileEditor = new PdfFileEditor();
                int[][] pages = new int[][] {

                    new int[] { startPage, endPage-1 }
                };

                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();
                MemoryStream[] filems = pdfFileEditor.SplitToBulks(inputFile, pages);
                stopwatch1.Stop();
                Console.WriteLine($"Convert cost time:{stopwatch1.ElapsedMilliseconds}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (var item in filems)
                {
                    using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        item.WriteTo(outputStream);
                    }

                }
                stopwatch.Stop();
                Console.WriteLine($"Save cost time:{stopwatch.ElapsedMilliseconds}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public static bool Extract(string inputFile, string outputPath, int startPage, int endPage)
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
            PdfFileEditor pfe = new PdfFileEditor();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            pfe.Extract(inputFile, startPage, endPage, outputPath);
            stopwatch.Stop();
            Console.WriteLine($"Convert and save cost time:{stopwatch.ElapsedMilliseconds}");
            return false;
        }
        public static bool Extract(string inputFile, string outputPath, int[] pages)
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
            PdfFileEditor pfe = new PdfFileEditor();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            pfe.Extract(inputFile, pages, outputPath);
            stopwatch.Stop();
            Console.WriteLine($"Convert and save cost time:{stopwatch.ElapsedMilliseconds}");
            return false;
        }
        public static bool ExtractStream(string inputFilePath, string outputPath, int[] pages)
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
            PdfFileEditor pfe = new PdfFileEditor();
            var res = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                // 创建输出文件流
                using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    // 临时存储提取页面的内存流
                    using (MemoryStream tempStream1 = new MemoryStream())
                    {
                        Console.WriteLine(value: $"ReadToStream cost time:{stopwatch.ElapsedMilliseconds}");
                        stopwatch.Restart();
                        // 提取第一个范围的页面并保存到内存流
                        res = pfe.Extract(inputStream, pages, tempStream1);
                        stopwatch.Stop();
                        Console.WriteLine(value: $"Convert cost time:{stopwatch.ElapsedMilliseconds}");
                        // 重置输入流位置以便再次读取
                        inputStream.Position = 0;

                        stopwatch.Restart();
                        // 将内存流中的内容写入输出文件流
                        tempStream1.WriteTo(outputStream);
                        stopwatch.Stop();
                        Console.WriteLine($"Write cost time:{stopwatch.ElapsedMilliseconds}");
                    }
                }
            }
            return res;
        }
        public static MemoryStream ExtractAsync(string inputFile, int startPage, int endPage)
        {
            PdfFileEditor pdfFileEditor = new PdfFileEditor();
            MemoryStream outputStream = new MemoryStream();

            using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                pdfFileEditor.Extract(inputStream, startPage, endPage, outputStream);
            }

            outputStream.Position = 0; // 重置内存流的位置，以便读取
            return outputStream;
        }
        public static void MulSectionExtraAsync(string inputFile, int[][] pageRange, string outputfile)
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<MemoryStream> tempStreams = new List<MemoryStream>();

            Parallel.ForEach(pageRange, (range, state, i) =>
            {
                var newoutputfile = Path.Combine(outputfile, "SplitPdf", $"DH_8105D738864_48935077_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE_{i}.pdf");

                MemoryStream tempStream;
                using (FileStream outputStream = new FileStream(newoutputfile, FileMode.Create, FileAccess.ReadWrite))
                {
                    tempStream = ExtractAsync(inputFile, range[0], range[1]);
                    tempStream.Position = 0;
                    tempStream.CopyTo(outputStream);
                    tempStreams.Add(tempStream);
                }
            });
            stopwatch.Stop();
            Console.WriteLine(value: $"Extra cost time: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            var newOutputfile = Path.Combine(outputfile, "Test.pdf");
            ConcatenatePdf(newOutputfile, tempStreams.ToArray());
            stopwatch.Stop();
            Console.WriteLine(value: $"Concat and save cost time: {stopwatch.ElapsedMilliseconds}");

        }
        /// <summary>
        /// 合并Pdf
        /// </summary>
        /// <param name="outputfile"></param>
        /// <param name="tempStreams"></param>
        public static void ConcatenatePdf(string outputfile, MemoryStream[] tempStreams)
        {
            using (FileStream outputStream2 = new FileStream(outputfile, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfFileEditor pdfEditor = new PdfFileEditor();
                pdfEditor.Concatenate(tempStreams.ToArray(), outputStream2);
            }
            // 关闭所有内存流
            foreach (var stream in tempStreams)
            {
                stream.Dispose();
            }
        }
    }
}
