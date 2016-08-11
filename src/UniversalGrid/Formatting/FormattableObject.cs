namespace UniversalGrid.Formatting
{
    public class FormattableObject<T>
    {
        public FormattableObject()
        {
            FontStyle = new FontStyle();
        }

        public FontStyle FontStyle { get; private set; }
    }
}
