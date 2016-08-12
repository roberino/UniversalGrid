using UniversalGrid.Drawing;

namespace UniversalGrid.Formatting
{
    public class FormattableObject<T>
    {
        public FormattableObject()
        {
            FontStyle = new FontStyle();
        }

        public FontStyle FontStyle { get; private set; }
        public Colour ForegroundColour { get; set; }
        public Colour BackgroundColour { get; set; }
        public Colour BorderColour { get; set; }
        public double BorderWidth { get; set; }
    }
}