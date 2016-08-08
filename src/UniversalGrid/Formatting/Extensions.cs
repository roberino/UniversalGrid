using System.IO;

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