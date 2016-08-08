namespace UniversalGrid.Drawing
{
    public struct Colour
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public bool IsTransparent
        {
            get
            {
                return A == 0;
            }
        }

        public string ToHex(bool includeAlpha = false)
        {
            if (includeAlpha)
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A).ToLower();

            return string.Format("#{0:X2}{1:X2}{2:X2}", R, G, B).ToLower();
        }

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
