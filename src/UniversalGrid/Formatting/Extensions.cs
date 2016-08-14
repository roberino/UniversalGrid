using System;
using System.IO;
using System.Xml.Linq;

namespace UniversalGrid.Formatting
{
    public static class Extensions
    {
        public static void FormatUsing<T>(this UniversalGrid<T> grid, ITextFormatter<T> formatter)
        {
            var writer = new GridWriter<T>(formatter);
            writer.Write(grid);
        }

        public static string ToHtml<T>(this UniversalGrid<T> grid, string tableClass = null)
        {
            using (var output = new StringWriter())
            {
                grid.ToHtml(output, tableClass);
                return output.ToString();
            }
        }

        public static XDocument ToXml<T>(this UniversalGrid<T> grid, Func<T, XNode> objectFormatter = null)
        {
            var doc = new XDocument();

            using (var output = doc.CreateWriter())
            {
                var htmlFormatter = new XmlFormatter<T>(output, objectFormatter ?? (x => x == null ? null : new XText(x.ToString())));
                var writer = new GridWriter<T>(htmlFormatter);
                writer.Write(grid);
            }

            return doc;
        }

        public static XDocument ToSvg<T>(this UniversalGrid<T> grid, Func<T, XNode> objectFormatter = null)
        {
            var doc = new XDocument();

            using (var output = doc.CreateWriter())
            {
                var htmlFormatter = new SvgFormatter<T>(output, objectFormatter ?? (x => x == null ? null : new XText(x.ToString())));
                var writer = new GridWriter<T>(htmlFormatter);
                writer.Write(grid);
            }

            return doc;
        }

        public static void ToHtml<T>(this UniversalGrid<T> grid, TextWriter output, string tableClass = null)
        {
            var htmlFormatter = new HtmlFormatter<T>(output, x => x.ToString(), tableClass);
            var writer = new GridWriter<T>(htmlFormatter);
            writer.Write(grid);
        }

        public static string ToCsv<T>(this UniversalGrid<T> grid, char delimitter = '\t')
        {
            using (var output = new StringWriter())
            {
                grid.ToCsv(output, delimitter);
                return output.ToString();
            }
        }

        public static void ToCsv<T>(this UniversalGrid<T> grid, TextWriter output, char delimitter = '\t')
        {
            var htmlFormatter = new CsvFormatter<T>(output, x => x.ToString(), delimitter);
            var writer = new GridWriter<T>(htmlFormatter);
            writer.Write(grid);
        }
    }
}