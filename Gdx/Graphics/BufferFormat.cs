namespace LibGDXSharp.Graphics
{
    public class BufferFormat
    {
        public int  R                { get; set; }
        public int  G                { get; set; }
        public int  B                { get; set; }
        public int  A                { get; set; }
        public int  Depth            { get; set; }
        public int  Stencil          { get; set; }
        public int  Samples          { get; set; }
        public bool CoverageSampling { get; set; }

        public BufferFormat( int  r, int g, int b, int a, int depth, int stencil, int samples,
                             bool coverageSampling )
        {
            this.R                = r;
            this.G                = g;
            this.B                = b;
            this.A                = a;
            this.Depth            = depth;
            this.Stencil          = stencil;
            this.Samples          = samples;
            this.CoverageSampling = coverageSampling;
        }

        public new string ToString()
        {
            return "r - " + R + ", g - " + G + ", b - " + B + ", a - " + A
                   + ", depth - " + Depth + ", stencil - " + Stencil
                   + ", num samples - " + Samples + ", coverage sampling - " + CoverageSampling;
        }
    }
}
