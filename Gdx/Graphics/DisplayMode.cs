namespace LibGDXSharp.Graphics
{
    public class DisplayMode
    {
        public int Width        { get; set; }
        public int Height       { get; set; }
        public int RefreshRate  { get; set; }
        public int BitsPerPixel { get; set; }

        public DisplayMode( int width, int height, int refreshRate, int bitsPerPixel )
        {
            this.Width        = width;
            this.Height       = height;
            this.RefreshRate  = refreshRate;
            this.BitsPerPixel = bitsPerPixel;
        }

        public new string ToString()
        {
            return Width + "x" + Height + ", bpp: " + BitsPerPixel + ", hz: " + RefreshRate;
        }
    }
}
