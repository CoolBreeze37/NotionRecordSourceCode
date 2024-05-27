using Aspose.Pdf.Facades;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_.异步编程_Parallel大批量文件创建
{
    /// <summary>
    /// pdf操作类
    /// </summary>
    public static class AsposePdf
    {
        public static readonly string BasePath = Path.Combine($"{Environment.CurrentDirectory}","PdfCache");
        public static readonly string PartialPdfPath = "Partial";
        /// <summary>
        /// 加载License
        /// </summary>
        static AsposePdf()
        {
            if (Directory.Exists(BasePath))
                Directory.Delete(BasePath,true);
            Directory.CreateDirectory(BasePath);
            new Aspose.Pdf.License().SetLicense("Aspose.Total.NET.lic");
        }
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
        /// <summary>
        /// 分割文件
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <returns></returns>
        public static bool ExtractPdf(PdfCase pdfCase)
        {
            try
            {
                var dirpath = Path.Combine($"{BasePath}", $"{pdfCase.Name}", $"{PartialPdfPath}");
                Directory.CreateDirectory(dirpath);
                Parallel.ForEach(pdfCase.PdfSKUSet, (item) =>
                {
                    var outputfilePath = Path.Combine(dirpath, item.Value.Name + ".pdf");
                    using (FileStream outputStream = new FileStream(outputfilePath, FileMode.Create, FileAccess.Write))
                    {
                        MemoryStream tempStream = Extract(pdfCase.PdfFilePath, item.Value.StartPage, item.Value.EndPage);
                        tempStream.Position = 0;
                        tempStream.WriteTo(outputStream);
                    }
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 合并pdf
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="outputfileName"></param>
        /// <param name="pdfNameList">无需后缀名.pdf</param>
        /// <param name="outputfilePath">返回一个合并后的文件路径</param>
        /// <returns></returns>
        public static bool ConcatenatePdf(string directoryName, string outputfileName, List<string> pdfNameList, out string outputfilePath)
        {
            outputfilePath = null;
            try
            {
                outputfilePath = Path.Combine($"{BasePath}", $"{directoryName}", outputfileName);
                var pdfPathList = new List<string>();
                foreach (string pdfName in pdfNameList)
                    pdfPathList.Add(Path.Combine($"{BasePath}", directoryName, $"{PartialPdfPath}", pdfName + ".pdf"));
                PdfFileEditor pdfEditor = new PdfFileEditor();
                pdfEditor.Concatenate(pdfPathList.ToArray(), outputfilePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
    /// <summary>
    /// Pdf工单类
    /// </summary>
    public class PdfCase
    {
        /// <summary>
        /// 源Pdf文件路径
        /// </summary>
        public string PdfFilePath {  get; set; }
        /// <summary>
        /// Case名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 记录当前工单是否已缓存所有SKU的pdf
        /// </summary>
        public bool HasCache { get; private set; }
        /// <summary>
        /// 所包含的SKU Key=PdfSKU.Name
        /// </summary>
        public Dictionary<string, PdfSKUPackage> PdfSKUSet { get; private set; } = new Dictionary<string, PdfSKUPackage>();
        /// <summary>
        /// 当前选择的SKU
        /// </summary>
        public List<string> SelectedSKUCollection { get; private set; } = new List<string>();
        public string SelectedPdfSKUMergePath { get;private set; }
        public PdfCase(string caseName,string pdfFilePath,List<PdfSKUPackage> pdfSKUList)
        {
            Name = caseName;
            PdfFilePath = pdfFilePath;
            FillPdfSKUToCase(pdfSKUList);
        }
        /// <summary>
        /// 向当前工单填充SKU列表
        /// </summary>
        /// <param name="pdfSKUList"></param>
        private void FillPdfSKUToCase(List<PdfSKUPackage> pdfSKUList)
        {
            foreach (var sku in pdfSKUList)
                PdfSKUSet.Add(sku.Name,sku);
        }
        /// <summary>
        /// 更新当前所选工单集合
        /// </summary>
        /// <param name="pdfSKUPackageNameList">PdfSKU Name</param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateSelectedSKUCollection(List<string> pdfSKUPackageNameList)
        {
            foreach (string item in pdfSKUPackageNameList)
            {
                if (!PdfSKUSet.ContainsKey(item))
                    throw new ArgumentException("Current sku is not belong to this Case.");
            }
            SelectedSKUCollection = pdfSKUPackageNameList.ToList();
        }
        /// <summary>
        /// 按照SKU对当前工单的pdf进行分割
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <returns></returns>
        public bool SplitAsSKU()
        {
            bool res = AsposePdf.ExtractPdf(this);
            if (res)
                HasCache = true;
            return res;
        }
        /// <summary>
        /// 合并指定SKU的Pdf文件
        /// </summary>
        /// <param name="selectedSKUFilePath"></param>
        /// <param name="outputfileName"></param>
        /// <param name="mergeFilePath"></param>
        /// <returns></returns>
        public bool ConcatenateSelectedPdfSKUs(out string mergeFilePath)
        {
            var outputfileName = $"{this.Name}_Merge.pdf";
            bool res = AsposePdf.ConcatenatePdf(this.Name, outputfileName, this.SelectedSKUCollection, out mergeFilePath);
            return res;
        }
    }
    /// <summary>
    /// PdfSKU类
    /// </summary>
    public class PdfSKUPackage
    {
        public readonly int StartPage;
        public readonly int EndPage;
        public readonly string Name;
        public readonly int Count;
        
        public PdfSKUPackage(string name, int startPage, int endPage)
        {
            StartPage = startPage;
            EndPage = endPage;
            Name = name;
            Count = endPage - startPage + 1;
        }
        
    }
}
