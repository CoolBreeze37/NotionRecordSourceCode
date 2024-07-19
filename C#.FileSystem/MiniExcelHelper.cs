using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace C_.FileSystem
{
    public static class MiniExcelHelper
    {
        public static void TestExportClassToCsv()
        {
            var config = new MiniExcelLibs.Csv.CsvConfiguration()
            {
                Seperator = ','
            };
            var path = "demo.csv";
            var values = new[] { new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true },
            new UserAccount() {  Name="1",Age=1, BoD = DateTime.Now, ID = Guid.NewGuid(), Points = 1, VIP = true }};
            MiniExcel.SaveAs(path,values,overwriteFile:true, configuration: config);

        }
    }
}
