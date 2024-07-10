using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C_.异步编程_Parallel大批量文件创建
{
    public static class iText7Pdf
    {
        #region 公有方法
        /// <summary>
        /// 合并pdf文件
        /// </summary>
        /// <param name="pdfPathList"></param>
        /// <param name="outputfilePath"></param>
        /// <returns></returns>
        public static bool ConcatenatePdf(List<string> pdfPathList, string outputfilePath)
        {
            try
            {
                MergePdfFiles(pdfPathList,outputfilePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        /// <summary>
        /// 旋转pdf文件
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <param name="targetpdfFilePath"></param>
        /// <param name="rotation"></param>        
        public static void RotatePagetotargetAngle(string pdfFilePath, string targetpdfFilePath, int rotation)
        {

            PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfFilePath), new PdfWriter(targetpdfFilePath));
            for (int p = 1; p <= pdfDocument.GetNumberOfPages(); p++)
            {
                PdfPage page = pdfDocument.GetPage(p);
                int rotate = page.GetRotation();
                if (rotate == 0)
                {
                    page.SetRotation(rotation);
                }
                else
                {
                    page.SetRotation((rotate + rotation) % 360);
                }
            }
            pdfDocument.Close();
        }
        /// <summary>
        /// 分割文件
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <param name="outPutDirPath"></param>
        /// <returns>0 成功 1取消 异常</returns>
        public static int ExtractPdf(PdfCase pdfCase,string outPutDirPath)
        {
            try
            {
                PdfDocument sourcePdfDoc = new PdfDocument(new PdfReader(pdfCase.PdfFilePath));
                List<string> outputfilePaths = pdfCase.PdfSKUSet.Select(x => Path.Combine(outPutDirPath, x.Value.Name + ".pdf")).ToList();

                ImprovedSplitter splitter = new ImprovedSplitter(sourcePdfDoc, outputfilePaths);

                var pdfDocs = splitter.ExtractPageRanges(pdfCase.PdfSKUSet.Select(x => new PageRange($"{x.Value.StartPage}-{x.Value.EndPage}")).ToList());
                foreach (var pdfDoc in pdfDocs)
                    pdfDoc.Close();
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
        #endregion

        #region 私有方法
        /// <summary>
        /// 合并Pdf文件
        /// </summary>
        /// <param name="srcFiles"></param>
        /// <param name="dest"></param>
        private static void MergePdfFiles(List<string> srcFiles, string dest)
        {
            // 配置 Bouncy Castle 作为 iText 的加密提供者
            // 创建输出PDF文件流
            using (PdfWriter writer = new PdfWriter(dest))
            {
                // 创建目标PDF文档
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer))
                {
                    // 创建PdfMerger实例
                    PdfMerger merger = new PdfMerger(pdfDoc);

                    // 遍历所有源PDF文件
                    foreach (string file in srcFiles)
                    {
                        // 创建源PDF文件流
                        using (PdfReader reader = new PdfReader(file))
                        {
                            // 创建源PDF文档
                            using (iText.Kernel.Pdf.PdfDocument srcDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                            {
                                // 将源PDF文档合并到目标PDF文档中
                                merger.Merge(srcDoc, 1, srcDoc.GetNumberOfPages());
                            }
                        }
                    }
                    merger.Close();
                }
            }
        }

        

        
        #endregion 
    }
    class ImprovedSplitter : PdfSplitter
    {
        IList<string> _outputPaths;
        int _partNumber = 0;


        public ImprovedSplitter(PdfDocument pdfDocument,IList<string> outputPaths) : base(pdfDocument)
        {
            _outputPaths = outputPaths;
        }
        
        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            return new PdfWriter(_outputPaths[_partNumber++]);
        }
        
    }
}
