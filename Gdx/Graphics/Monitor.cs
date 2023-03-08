namespace LibGDXSharp.Graphics
{
    public class Monitor
    {
        public int    VirtualX { get; set; }
        public int    VirtualY { get; set; }
        public string Name     { get; set; }

        public Monitor( int virtualX, int virtualY, string name )
        {
            this.VirtualX = virtualX;
            this.VirtualY = virtualY;
            this.Name     = name;
        }
    }
}
