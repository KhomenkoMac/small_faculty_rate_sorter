using Docnet.Core;
using Docnet.Core.Models;
using System.Text;

namespace RateDefiner
{
    public static class PDFExtractor
    {
        private static StringBuilder _stringBuilder = new();

        public static string PDFText(string path)
        {
            using (var docReader = DocLib.Instance.GetDocReader(path, new PageDimensions()))
            {
                for (var i = 0; i < docReader.GetPageCount(); i++)
                {
                    using var pageReader = docReader.GetPageReader(i);
                    _stringBuilder.Append(pageReader.GetText());
                }
            }
            return _stringBuilder.ToString();
        }
    }
}
