using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.CSV去科学计数导出
{
    /// <summary>
    /// 对于Access 数据量大时会很慢，后边换了Sqlite数据库
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceCsvPath = "D:\\Desktop\\Data\\8105D744768-49049195\\8105D744768-49049195.csv";
            string targetCsvPath = "D:\\Desktop\\target.csv";
            DataTable dataTable = new DataTable();

            // 读取 CSV 文件到 DataTable
            using (var reader = new StreamReader(sourceCsvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dataTable.Load(dr);
                }
            }

            // 写入 DataTable 到目标 CSV 文件
            using (var writer = new StreamWriter(targetCsvPath))
            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = (args1) => true // 强制所有字段加引号
            }))
            {
                // 写入头部信息
                foreach (DataColumn column in dataTable.Columns)
                {
                    csvWriter.WriteField(column.ColumnName);
                }
                csvWriter.NextRecord();

                // 写入数据行
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var value = row[column]?.ToString() ?? string.Empty;
                        csvWriter.WriteField(value);
                    }
                    csvWriter.NextRecord();
                }
            }

            Console.WriteLine("CSV 文件已成功转换并避免了科学计数法。");
        }
    }
}
