using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled
{
    public class TiledMapImageLayer : MapLayer
    {
        public TextureRegion Region { get; set; }
        public float         X      { get; set; }
        public float         Y      { get; set; }

        public TiledMapImageLayer( TextureRegion region, float x, float y )
        {
            this.Region = region;
            this.X      = x;
            this.Y      = y;
        }
    }
}
