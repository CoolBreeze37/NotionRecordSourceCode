using Aspose.Pdf;
using Aspose.Pdf.Facades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace C_.异步编程_Parallel大批量文件创建
{
    /// <summary>
    /// pdf操作类
    /// </summary>
    public static class AsposePdf
    {
        /// <summary>
        /// 加载License
        /// </summary>
        static AsposePdf()
        {
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
        }
        #region 公有方法
        /// <summary>
        /// 分割文件
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <param name="token"></param>
        /// <param name="outPutDirPath"></param>
        /// <returns>0 成功 1取消 异常</returns>
        public static int ExtractPdf(PdfCase pdfCase, CancellationToken token, string outPutDirPath)
        {
            try
            {
                ParallelOptions parallelOptions = new ParallelOptions() { CancellationToken = token, MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
                Parallel.ForEach(pdfCase.PdfSKUSet, parallelOptions, (item) =>
                {
                    var outputfilePath = Path.Combine(outPutDirPath, item.Value.Name + ".pdf");
                    using (FileStream outputStream = new FileStream(outputfilePath, FileMode.Create, FileAccess.Write))
                    {
                        MemoryStream tempStream = Extract(pdfCase.PdfFilePath, item.Value.StartPage, item.Value.EndPage);
                        tempStream.Position = 0;
                        tempStream.WriteTo(outputStream);
                    }
                });
                return 0;
            }
            catch (OperationCanceledException ex)
            {
                //取消操作
                return -1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 合并pdf
        /// </summary>
        /// <param name="pdfPathList">pdf文件路径</param>
        /// <param name="outputfilePath">返回一个合并后的文件路径</param>
        /// <returns></returns>
        public static bool ConcatenatePdf(List<string> pdfPathList, string outputfilePath)
        {
            try
            {
                PdfFileEditor pdfEditor = new PdfFileEditor();
                pdfEditor.CloseConcatenatedStreams = true;
                pdfEditor.Concatenate(pdfPathList.ToArray(), outputfilePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// 目前旋转速度最优
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <param name="targetpdfFilePath"></param>
        /// <param name="rotation"></param>
        public static void RotatePagetotargetAngle(string pdfFilePath, string targetpdfFilePath, Rotation rotation)
        {
            Document document = new Document(pdfFilePath);
            // 遍历 PDF 的每一页
            foreach (var page in document.Pages)
            {
                page.Rotate = rotation;
            }
            document.Save(targetpdfFilePath);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 提取文件
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="startPage"></param>
        /// <param name="endPage"></param>
        /// <returns></returns>
        private static MemoryStream Extract(string inputFile, int startPage, int endPage)
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
        #endregion
    }
}
