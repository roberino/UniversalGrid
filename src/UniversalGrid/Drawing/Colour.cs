namespace UniversalGrid.Drawing
{
    public struct Colour
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public static Colour Black
        {
            get
            {
                return new Colour()
                {
                    A = 255
                };
            }
        }
    }
}
