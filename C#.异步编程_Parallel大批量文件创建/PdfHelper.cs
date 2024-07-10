using Aspose.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace C_.异步编程_Parallel大批量文件创建
{
    /// <summary>
    /// pdf操作类
    /// </summary>
    public static class PdfHelper
    {
        private static Dictionary<string, PdfCase> caseCache = new Dictionary<string, PdfCase>();

        public static readonly string BasePath = Path.Combine($"{Environment.CurrentDirectory}", "PdfCache");
        //public static readonly string BasePath = Path.Combine(@"D:\", "PdfCache");
        public static readonly string PartialPdfPath = "Partial";

        static PdfHelper()
        {
            if (Directory.Exists(BasePath))
                Directory.Delete(BasePath, true);
            Directory.CreateDirectory(BasePath);
        }
        public static Dictionary<string, PdfCase> CaseCache { get { return caseCache; } }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>
        public static void ClearCache()
        {
            //清除所有缓存
            caseCache.Clear();
            //删除所有缓存记录
            if (Directory.Exists(BasePath))
                Directory.Delete(BasePath, true);
            Directory.CreateDirectory(BasePath);
        }
        public static bool HasCache(string caseName)
        {
            if (caseCache.ContainsKey(caseName))
            {
                if (caseCache[caseName].HasCache)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取PdfCase
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="pdfFilePath"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static PdfCase GetPdfCase(string caseName, string pdfFilePath, DataTable dt)
        {
            var dic = GroupBySKU(dt);
            List<PdfSKUPackage> pdfSKUPackages = new List<PdfSKUPackage>();
            foreach (var kp in dic)
            {
                int startPage = Convert.ToInt32(kp.Value.Min(x => Convert.ToInt32(x.PAGE_NUMBER)));
                int endPage = Convert.ToInt32(kp.Value.Max(x => Convert.ToInt32(x.PAGE_NUMBER)));
                pdfSKUPackages.Add(new PdfSKUPackage(kp.Key, startPage, endPage));
            }
            PdfCase pdfCase = new PdfCase(caseName, pdfFilePath, pdfSKUPackages);
            pdfCase.BindSKUUnits(dic);
            caseCache[caseName] = pdfCase;
            return pdfCase;
        }
        /// <summary>
        /// 按照SKU对Excel中的数据进行分类
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static Dictionary<string, List<SKUUnitData>> GroupBySKU(DataTable dataTable)
        {
            // 使用LINQ按SKU分组，并选择所需列
            var groupedData = from row in dataTable.AsEnumerable()
                              group row by row.Field<string>("SKU") into skuGroup
                              select new
                              {
                                  SKU = skuGroup.Key,
                                  Items = skuGroup.Select(r => new SKUUnitData
                                  {
                                      SKU = r.Field<string>("SKU"),
                                      EPC = r.Field<string>("EPC"),
                                      RESULT = r.Field<Int16>("RESULT"),
                                      PAGE_NUMBER = r.Field<string>("PAGE_NUMBER"),
                                      SORTING_SEQ = r.Field<string>("SORTING_SEQ")
                                  }).ToList()
                              };

            // 创建字典，其中键是SKU，值是List<Data>
            Dictionary<string, List<SKUUnitData>> dataDictionary = groupedData.ToDictionary(g => g.SKU, g => g.Items);
            return dataDictionary;
        }
        /// <summary>
        /// 从CSV中读取数据并填充到DataTable中
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCSV(string path)
        {
            DataTable dataTable = new DataTable();
            List<string> columns;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            // 填充数据到DataTable
            using (var reader = new StreamReader(fileStream))
            {
                var line = reader.ReadLine();
                columns = line.Split(',').ToList();
                foreach (string column in columns)
                {
                    if (column.Equals("RESULT"))
                        dataTable.Columns.Add(column, typeof(Int16));
                    else
                        dataTable.Columns.Add(column, typeof(string));
                }
                while (!reader.EndOfStream)
                {
                    string dataLine = reader.ReadLine();
                    string[] values = dataLine.Split(',');
                    dataTable.Rows.Add(values);
                }
            }
            return dataTable;
        }
        #region Aspose
        /// <summary>
        /// Aspose 将pdf旋转指定角度
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="pdfFilePath"></param>
        /// <param name="outPutPdfPath"></param>
        /// <param name="rotateAngle"></param>
        public static void Aspose_RotatePdftoTargetAngle(string caseName, string pdfFilePath, out string outPutPdfPath, Rotation rotateAngle)
        {
            var name = Path.GetFileName(pdfFilePath);
            string directoryPath = Path.Combine(BasePath, caseName);
            Directory.CreateDirectory(directoryPath);
            outPutPdfPath = Path.Combine(directoryPath, name);
            AsposePdf.RotatePagetotargetAngle( pdfFilePath, outPutPdfPath, rotateAngle);
        }
        /// <summary>
        /// Aspose 提取pdf
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static int Aspose_ExtractPdf(PdfCase pdfCase, CancellationToken cancellationToken)
        {
            string outPutDirPath = Path.Combine($"{BasePath}", $"{pdfCase.Name}", $"{PartialPdfPath}");
            Directory.CreateDirectory(outPutDirPath);
            return AsposePdf.ExtractPdf(pdfCase, cancellationToken, outPutDirPath);
        }
        /// <summary>
        /// Aspose 合并pdf
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="outputfileName"></param>
        /// <param name="pdfNameList"></param>
        /// <param name="outputfilePath"></param>
        /// <returns></returns>
        public static bool Aspose_ConcatenatePdf(string directoryName, string outputfileName, List<string> pdfNameList, out string outputfilePath)
        {
            var _outputfilePath = Path.Combine($"{BasePath}", $"{directoryName}", outputfileName);
            var pdfPathList = new List<string>();
            foreach (string pdfName in pdfNameList)
                pdfPathList.Add(Path.Combine($"{BasePath}", directoryName, $"{PartialPdfPath}", pdfName + ".pdf"));
            var res = AsposePdf.ConcatenatePdf(pdfPathList, _outputfilePath);
            if(res)
                outputfilePath = _outputfilePath;
            else
                outputfilePath = null;
            return res;
        }
        #endregion

        #region iText7
        /// <summary>
        /// iText7 将pdf旋转指定角度
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="pdfFilePath"></param>
        /// <param name="outPutPdfPath"></param>
        /// <param name="rotateAngle"></param>
        public static void iText7_RotatePdftoTargetAngle(string caseName, string pdfFilePath, out string outPutPdfPath, int rotateAngle)
        {
            var name = Path.GetFileName(pdfFilePath);
            string directoryPath = Path.Combine(BasePath, caseName);
            Directory.CreateDirectory(directoryPath);
            outPutPdfPath = Path.Combine(directoryPath, name);
            iText7Pdf.RotatePagetotargetAngle( pdfFilePath, outPutPdfPath, rotateAngle);
        }
        /// <summary>
        /// iText7 提取pdf
        /// </summary>
        /// <param name="pdfCase"></param>
        /// <returns></returns>
        public static int iText7_ExtractPdf(PdfCase pdfCase)
        {
            string outPutDirPath = Path.Combine($"{BasePath}", $"{pdfCase.Name}", $"{PartialPdfPath}");
            Directory.CreateDirectory(outPutDirPath);
            return iText7Pdf.ExtractPdf(pdfCase,outPutDirPath);
        }
        /// <summary>
        /// iText7 合并pdf
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="outputfileName"></param>
        /// <param name="pdfNameList"></param>
        /// <param name="outputfilePath"></param>
        /// <returns></returns>
        public static bool iText7_ConcatenatePdf(string directoryName, string outputfileName, List<string> pdfNameList, out string outputfilePath)
        {
            var _outputfilePath = Path.Combine($"{BasePath}", $"{directoryName}", outputfileName);
            var pdfPathList = new List<string>();
            foreach (string pdfName in pdfNameList)
                pdfPathList.Add(Path.Combine($"{BasePath}", directoryName, $"{PartialPdfPath}", pdfName + ".pdf"));
            var res =  iText7Pdf.ConcatenatePdf(pdfPathList, _outputfilePath);
            if (res)
                outputfilePath = _outputfilePath;
            else
                outputfilePath = null;
            return res;
        }
        #endregion
    }
}
