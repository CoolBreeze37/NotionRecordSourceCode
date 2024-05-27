using Aspose.Pdf.Plugins;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        public static void JpegConverter()
        {
            // Create a new instance of Jpeg
            var converter = new Jpeg();
            // Specify the input and output file paths
            var inputPath = Path.Combine(@"C:\Samples\", "sample.pdf");
            var outputPath = Path.Combine(@"C:\Samples\", "images");

            // Create an instance of the JpegOptions class
            var converterOptions = new JpegOptions();

            // Add the input and output file paths to the options
            converterOptions.AddInput(new FileDataSource(inputPath));
            converterOptions.AddOutput(new FileDataSource(outputPath));
            // Set the output resolution to 300 dpi
            converterOptions.OutputResolution = 300;

            // Set the page range to the first page
            converterOptions.PageRange = new PageRange(1);
            // Process the conversion and get the result container
            ResultContainer resultContainer = converter.Process(converterOptions);

            // Print the status of the conversion
            Console.WriteLine(resultContainer.Status);

            // Print the paths of the converted JPEG images
            foreach (FileResult operationResult in resultContainer.ResultCollection.Cast<FileResult>())
            {
                Console.WriteLine(operationResult.Data.ToString());
            }
        }
    }
}
