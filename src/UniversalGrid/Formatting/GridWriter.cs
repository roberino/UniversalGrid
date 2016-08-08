namespace UniversalGrid.Formatting
{
    public class GridWriter<T>
    {
        private readonly ITextFormatter<T> _textFormatter;

        public GridWriter(ITextFormatter<T> textFormatter)
        {
            _textFormatter = textFormatter;
        }

        public void Write(UniversalGrid<T> grid)
        {
            var r = grid.TopLeft.Y - 1;
            var c = -1;

            _textFormatter.WriteStartGrid();

            grid.Render((p, x) =>
            {
                if (p.Y > r)
                {
                    if(r > -1)
                    {
                        _textFormatter.WriteEndRow();
                    }
                    _textFormatter.WriteStartRow(p.Y - grid.TopLeft.Y);
                    c = 0;
                }

                _textFormatter.WriteCell(c, x);

                c++;
                r = p.Y;    
            });

            if (r > -1)
            {
                _textFormatter.WriteEndRow();
            }

            _textFormatter.WriteEndGrid();
        }
    }
}
