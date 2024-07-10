using Aspose.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C_.异步编程_Parallel大批量文件创建
{
    /// <summary>
    /// Pdf工单类
    /// </summary>
    public class PdfCase
    {
        public bool IsSelected { get; set; }
        /// <summary>
        /// 源Pdf文件路径
        /// </summary>
        public string PdfFilePath { get; set; }
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
        private string selectedPdfSKUMerge_AbsolutePath;
        private string selectedPdfSKUMerge_RelativePath;
        /// <summary>
        /// 合并好pdf路径位置
        /// </summary>
        public string SelectedPdfSKUMerge_AbsolutePath
        {
            get { return selectedPdfSKUMerge_AbsolutePath; }
            private set
            {
                selectedPdfSKUMerge_AbsolutePath = value;
                selectedPdfSKUMerge_RelativePath = GetRelativePath(PdfHelper.BasePath, SelectedPdfSKUMerge_AbsolutePath);
            }
        }
        public string SelectedPdfSKUMerge_RelativePath
        {
            get { return selectedPdfSKUMerge_RelativePath; }
        }
        public Dictionary<string, List<SKUUnitData>> SKUUnits { get; private set; }
        public Dictionary<string, SKUUnitData> SelectedSKUUnitsInfo { get; private set; }


        public PdfCase(string caseName, string pdfFilePath, List<PdfSKUPackage> pdfSKUList)
        {
            Name = caseName;
            PdfFilePath = pdfFilePath;
            FillPdfSKUToCase(pdfSKUList);
        }
        public void UpdateSelectedSKUUnitsInfo(Dictionary<string, SKUUnitData> unitsInfo)
        {
            SelectedSKUUnitsInfo = unitsInfo;
        }
        public void BindSKUUnits(Dictionary<string, List<SKUUnitData>> units)
        {
            SKUUnits = units;
        }
        /// <summary>
        /// 向当前工单填充SKU列表
        /// </summary>
        /// <param name="pdfSKUList"></param>
        private void FillPdfSKUToCase(List<PdfSKUPackage> pdfSKUList)
        {
            foreach (var sku in pdfSKUList)
                PdfSKUSet.Add(sku.Name, sku);
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
        /// <param name="cancellationToken"></param>
        /// <returns>0成功 1取消 异常</returns>
        public int SplitBySKU(CancellationToken cancellationToken)
        {
            try
            {
                //int res = AsposePdf.ExtractPdf(this, cancellationToken);
                int res = PdfHelper.iText7_ExtractPdf(this);
                if (res == 0)
                {
                    HasCache = true;
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 合并指定SKU的Pdf文件,同时更新Pdf页码
        /// </summary>
        /// <param name="mergeFilePath"></param>
        /// <param name="rotate"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool ConcatenateSelectedPdfSKUs(out string mergeFilePath, bool rotate, Rotation rotation)
        {
            var outputfileName = $"{this.Name}_Merge.pdf";
            //bool res = PdfHelper.Aspose_ConcatenatePdf(this.Name, outputfileName, this.SelectedSKUCollection, out mergeFilePath);
            bool res = PdfHelper.iText7_ConcatenatePdf(this.Name, outputfileName, this.SelectedSKUCollection, out mergeFilePath);
            //更新Pdf绝对路径
            SelectedPdfSKUMerge_AbsolutePath = mergeFilePath;
            //更新页码映射
            UpdatePdfPageNum();
            return res;
        }
        /// <summary>
        /// 暂时这么写，后边要优化 更新所选SKU的页码
        /// </summary>
        private void UpdatePdfPageNum()
        {
            // 选择包含在SelectedSKUCollection中的SKUUnits
            Dictionary<string, List<SKUUnitData>> SelectedSKUUnits = new Dictionary<string, List<SKUUnitData>>();
            SelectedSKUUnits = this.SKUUnits
                .Where(x => this.SelectedSKUCollection.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            // 对选择的SKUUnits按照PageNum进行分组
            Dictionary<string, Dictionary<string, List<SKUUnitData>>> groupedDict = SelectedSKUUnits.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                    .GroupBy(skuData => skuData.PAGE_NUMBER)
                    .ToDictionary(g => g.Key, g => g.ToList())
            );

            // 合并所有SKUUnitData
            var combinedList = groupedDict.SelectMany(outerKvp => outerKvp.Value.Values).ToList();

            // 更新页码并构建新的字典
            var selectedSKUUnitsInfo = new Dictionary<string, SKUUnitData>();
            int i = 1;
            foreach (var item in combinedList)
            {
                foreach (var subItem in item)
                {
                    subItem.NewPAGE_NUMBER = i.ToString();
                    selectedSKUUnitsInfo[subItem.EPC] = subItem;
                }
                i++;
            }
            this.UpdateSelectedSKUUnitsInfo(selectedSKUUnitsInfo);
        }
        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetRelativePath(string basePath, string fullPath)
        {
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                basePath += Path.DirectorySeparatorChar;
            }

            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);

            Uri relativeUri = baseUri.MakeRelativeUri(fullUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
