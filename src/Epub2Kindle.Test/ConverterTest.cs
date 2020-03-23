using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epub2Kindle.Test
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void ConvertEpub()
        {
            var converter = new Converter();
            var folder = @"C:\_epub2kindle";
            var inputFile = "book.epub";
            var outputFile = "book.mobi";
            var result = converter.Convert(folder, inputFile, outputFile);

            Assert.IsTrue(result);
        }
    }
}
