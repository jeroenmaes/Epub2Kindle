using Kindlegen;
using Kindlegen.Models;
using System;
using System.IO;
using System.Linq;

namespace Epub2Kindle
{
    public class Converter
    {
        public bool Convert(string folder, string intputFileName, string outputFileName)
        {
            var inputPath = Path.Combine(folder, intputFileName);

            var result = KindleConverter.Create(inputPath)
                .SetCompressionLevel(CompressionLevel.NoCompression)
                .SetOutput(outputFileName)
                .Convert();

            if (!result.IsSuccess)
            {
                string message = result.Details.FirstOrDefault(x => x.ConvertLogType == ConvertLogType.Error)?.Message;
                Console.WriteLine($"Has exception: {message}");
            }
            else
            {
                Console.WriteLine("Complete successfully");
            }

            return result.IsSuccess;
        }
    }
}
