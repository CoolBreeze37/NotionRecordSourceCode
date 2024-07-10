using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.异步编程_Parallel大批量文件创建
{
    /// <summary>
    /// PdfSKU类
    /// </summary>
    public class PdfSKUPackage
    {
        /// <summary>
        /// 起始页
        /// </summary>
        public readonly int StartPage;
        /// <summary>
        /// 终止页
        /// </summary>
        public readonly int EndPage;
        /// <summary>
        /// SKU Name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// 总页数
        /// </summary>
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
