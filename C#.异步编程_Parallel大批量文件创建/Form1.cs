using Aspose.Pdf;
using Aspose.Pdf.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_.异步编程_Parallel大批量文件创建
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //耗费时间：2m20s   
        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();

                Parallel.For(1, 100000, pageNumber =>
                {
                    string filePath = $"File{pageNumber}.txt";
                    using (StreamWriter imageStream = new StreamWriter(filePath, true))
                    {
                        imageStream.Write("fdafsdfdsafdsafs");
                    }
                    Console.WriteLine($"Created file: {filePath}");
                });
                sw.Start();
                Console.WriteLine($"Time:{sw.ElapsedMilliseconds}");
            });


        }
        //耗费时间：30s  结论：当文件名是纯数字时耗费时间更少 目前只尝试了txt
        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();

                Parallel.For(1, 100000, pageNumber =>
                {
                    string filePath = Path.Combine(pageNumber + ".txt");
                    using (StreamWriter imageStream = new StreamWriter(filePath, true))
                    {
                        imageStream.Write("fdafsdfdsafdsafs");
                    }
                    Console.WriteLine($"Created file: {filePath}");
                });
                Console.WriteLine($"Time:{sw.ElapsedMilliseconds}");

            });

        }
        //耗费时间：30s  
        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();

                Parallel.For(1, 100000, async pageNumber =>
                {
                    string filePath = Path.Combine(pageNumber + ".txt");
                    await WriteFileAsync(filePath, "fdafsdfdsafdsafs");
                    Console.WriteLine($"Created file: {filePath}");
                });
                Console.WriteLine($"Time:{sw.ElapsedMilliseconds}");

            });

        }

        static async Task WriteFileAsync(string filePath, string content)
        {
            StreamWriter imageStream = new StreamWriter(filePath, true);

            await imageStream.WriteAsync(content);
            imageStream.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                AsposePdftoPng.Convert();
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task.Run(() =>{
                AsposePdftoPng.JpegConverter();
            });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                SpirePdfToImage.Convert();
            });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                string inputFilePath = Path.Combine(@"D:\Desktop\PdftoPng", "DH_8105D738864_48935077_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE.pdf");
                string outFilePath = Path.Combine(@"D:\Desktop\PdftoPng\SplitPdf", "test.pdf");
                //方法1
                //SplitPdf.Split(inputFilePath, outFilePath,1,40000);
                int[] range1 = Enumerable.Range(10, 2000 - 10 + 1).ToArray();
                // 生成从300到1000的数组
                int[] range2 = Enumerable.Range(3000, 10000 - 3000 + 1).ToArray();
                int[] range3 = Enumerable.Range(11000, 40000 - 11000 + 1).ToArray();
                int[] pages = range1.Concat(range2).Concat(range3).ToArray();
                //方法2
                //SplitPdf.Extract(inputFilePath, outFilePath, pages);
                //方法3
                //SplitPdf.ExtractStream(inputFilePath, outFilePath, pages);

                //方法3多线程
                int[][] pageRange = new int[][] { 
                    new int[] { 1,     2806  }, 
                    new int[] { 2807,  3212  }, 
                    new int[] { 3213,  8073  },
                    new int[] { 8074,  15829 },
                    new int[] { 15830, 20195 },
                    new int[] { 20196, 25986 },
                    new int[] { 25987, 29182 },
                    new int[] { 29183, 29378 },
                    new int[] { 29379, 29494 },
                    new int[] { 29495, 31655 },
                    new int[] { 31656, 32621 },
                    new int[] { 32622, 35142 },
                    new int[] { 35143, 35298 },
                    new int[] { 35299, 36784 },
                    new int[] { 36785, 37195 },
                    new int[] { 37196, 37601 },
                    new int[] { 37602, 38362 },
                    new int[] { 38363, 39143 },
                    new int[] { 39144, 39359 },
                    new int[] { 39360, 41770 },
                    new int[] { 41771, 42251 },
                    new int[] { 42252, 43322 },
                    new int[] { 43323, 45198 },
                    new int[] { 45199, 45554 },
                    new int[] { 45555, 45685 }
                };

                int spr = 500;

                int count = (45685 / spr) + 1;
                int mod = 45685 % spr;
                int[][] newPageRange = new int[count][];
                int index = 1;

                for (int j = 0; j <= count-1; j++)
                {
                    if (count-1 == j)
                    {
                        newPageRange[j] = new int[2] { index, index + mod };
                    }
                    else
                    {
                        newPageRange[j] = new int[2] { index, index + spr - 1 };
                    }
                    index += spr;
                }
                
                string baseOutFilePath = Path.Combine(@"D:\Desktop\PdftoPng");
                SplitPdf.MulSectionExtraAsync(inputFilePath, pageRange, baseOutFilePath);
                
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                List<PdfSKUPackage> pdfSKUPackages = new List<PdfSKUPackage>
            {
                new PdfSKUPackage("200024361872",1    ,2806),
                new PdfSKUPackage("200024361873",2807 ,3212),
                new PdfSKUPackage("200024361874",3213 ,8073),
                new PdfSKUPackage("200024361875",8074 ,15829),
                new PdfSKUPackage("200024361876",15830,20195),
                new PdfSKUPackage("200024361877",20196,25986),
                new PdfSKUPackage("200024361878",25987,29182),
                new PdfSKUPackage("200024361879",29183,29378),
                new PdfSKUPackage("200024361880",29379,29494),
                new PdfSKUPackage("200024361881",29495,31655),
                new PdfSKUPackage("200024361882",31656,32621),
                new PdfSKUPackage("200024361883",32622,35142),
                new PdfSKUPackage("200024361884",35143,35298),
                new PdfSKUPackage("200024361885",35299,36784),
                new PdfSKUPackage("200024361886",36785,37195),
                new PdfSKUPackage("200024361887",37196,37601),
                new PdfSKUPackage("200024361888",37602,38362),
                new PdfSKUPackage("200024361889",38363,39143),
                new PdfSKUPackage("200024361890",39144,39359),
                new PdfSKUPackage("200024361891",39360,41770),
                new PdfSKUPackage("200024361892",41771,42251),
                new PdfSKUPackage("200024361893",42252,43322),
                new PdfSKUPackage("200024361894",43323,45198),
                new PdfSKUPackage("200024361895",45199,45554),
                new PdfSKUPackage("200024361896",45555,45685)
            };
                string pdfFilePath = "D:\\Work\\RFID\\Encopro-SML2019\\RFID写码\\bin\\Debug\\DataParameter\\8105D738864-48935077\\DH_8105D738864_48935077_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE.pdf";
                PdfCase pdfCase = new PdfCase("8105D738864", pdfFilePath, pdfSKUPackages);
                CancellationTokenSource cts = new CancellationTokenSource();
                int res = pdfCase.SplitBySKU(cts.Token);
                List<string> selectedSKUList = new List<string>
            {
                "200024361872",
                "200024361873",
                "200024361874",
                "200024361875",
                "200024361876",
                "200024361877",
                "200024361878",
                "200024361879",
                "200024361880",
                "200024361881",
                "200024361882",
                "200024361883",
                "200024361884",
                "200024361885",
                "200024361886",
                "200024361887",
                "200024361888",
                "200024361889",
                "200024361890",
                "200024361891",
                "200024361892",
                "200024361893",
                "200024361894",
                "200024361895",
                "200024361896"
            };
                pdfCase.UpdateSelectedSKUCollection(selectedSKUList);
                string mpath;
                if (res == 0)
                    _ = pdfCase.ConcatenateSelectedPdfSKUs(out mpath, false, Rotation.None);
            });
            

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var dt = GetDataTableFromCSV("D:\\Desktop\\RFID_Data\\PdftoPngTest\\8105D738864-48935077\\8105D738864-48935077.csv");
            string pdfFilePath = "D:\\Desktop\\RFID_Data\\PdftoPngTest\\8105D738864-48935077\\DH_8105D738864_48935077_HMHMM9V0BX001_HMHMM9V0BX_3_S_NOPLATE.pdf";
            PdfHelper.iText7_RotatePdftoTargetAngle("8105D738864-48935077", pdfFilePath, out string targetpdfFilePath, 90);
            var pdfCase = GetPdfCase("8105D738864-48935077", pdfFilePath, dt);
            CancellationTokenSource cts = new CancellationTokenSource();
            int res = pdfCase.SplitBySKU(cts.Token);
            List<string> selectedSKUList = new List<string>
            {
                "200024361872",
                "200024361873",
                "200024361874",
                "200024361875",
                "200024361876",
                "200024361877",
                "200024361878",
                "200024361879",
                "200024361880",
                "200024361881",
                "200024361882",
                "200024361883",
                "200024361884",
                "200024361885",
                "200024361886",
                "200024361887",
                "200024361888",
                "200024361889",
                "200024361890",
                "200024361891",
                "200024361892",
                "200024361893",
                "200024361894",
                "200024361895",
                "200024361896"
            };

            if (res==0)
            {
                 pdfCase.UpdateSelectedSKUCollection(selectedSKUList);
                _ = pdfCase.ConcatenateSelectedPdfSKUs(out string mpath,false,Rotation.None);
                Console.WriteLine(mpath);
            }


        }
        
        private PdfCase GetPdfCase(string caseName,string pdfFilePath,DataTable dt)
        {
            var dic = GroupBySKU(dt);
            List<PdfSKUPackage> pdfSKUPackages = new List<PdfSKUPackage>();
            foreach (var kp in dic)
            {
                int startPage = Convert.ToInt32(kp.Value.Min(x => Convert.ToInt32(x.PAGE_NUMBER)));
                int endPage = Convert.ToInt32(kp.Value.Max(x => Convert.ToInt32(x.PAGE_NUMBER)));
                pdfSKUPackages.Add(new PdfSKUPackage(kp.Key, startPage, endPage));
            }
            PdfCase pdfCase = new PdfCase(caseName, pdfFilePath,pdfSKUPackages);
            return pdfCase;
        }
        private Dictionary<string, List<Data>> GroupBySKU(DataTable dataTable)
        {
            // 使用LINQ按SKU分组，并选择所需列
            var groupedData = from row in dataTable.AsEnumerable()
                              group row by row.Field<string>("SKU") into skuGroup
                              select new
                              {
                                  SKU = skuGroup.Key,
                                  Items = skuGroup.Select(r => new Data
                                  {
                                      SKU = r.Field<string>("SKU"),
                                      RESULT = r.Field<Int16>("RESULT"),
                                      PAGE_NUMBER = r.Field<string>("PAGE_NUMBER"),
                                      SORTING_SEQ = r.Field<string>("SORTING_SEQ")
                                  }).ToList()
                              };

            // 创建字典，其中键是SKU，值是List<Data>
            Dictionary<string, List<Data>> dataDictionary = groupedData.ToDictionary(g => g.SKU, g => g.Items);
            return dataDictionary;
        }

        public DataTable GetDataTableFromCSV(string path)
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

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(5);
                textBox1.Text = $"{i}";
            }
         
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            // Start a task to cancel the operation after a delay
            Task.Run(() =>
            {
                Thread.Sleep(2000); // Adjust the delay as needed
                cts.Cancel();
            });

            try
            {
                Parallel.For(0, 100, new ParallelOptions { CancellationToken = token }, (i, state) =>
                {
                    string filePath = $"file{i}.txt";
                    StreamWriter writer = null;

                    try
                    {
                        writer = new StreamWriter(filePath);
                        writer.WriteLine($"Writing to file {i}");

                        // Simulate some work
                        Thread.Sleep(500);

                        // Check for cancellation
                        token.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Operation cancelled at iteration {i}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception at iteration {i}: {ex.Message}");
                    }
                    finally
                    {
                        writer?.Dispose(); // Ensure the file is closed and the resource is released
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Parallel.For operation was cancelled.");
            }

            Console.WriteLine("Completed");
        }
    }
    public class Data
    {
        public string SKU { get; set; }
        public Int16 RESULT { get; set; }
        public string PAGE_NUMBER { get; set; }
        public string SORTING_SEQ { get; set; }
    }
}
