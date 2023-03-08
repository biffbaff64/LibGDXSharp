namespace LibGDXSharp.Maps.Tiled
{
    public class TiledMap
    {
        public TiledMapTileSets Tilesets { get; set; }

        private List< object >? _ownedResources;
        
        public TiledMap()
        {
            Tilesets = new TiledMapTileSets();
        }

        public void SetOwnedResources( List< object > resources )
        {
            _ownedResources = resources;
        }
    }
}

