namespace LibGDXSharp.Maps.Tiled.Renderers
{
    public class OrthogonalTiledMapRenderer : BatchTileMapRenderer
    {
        private TiledMap? _tiledMap;

        public OrthogonalTiledMapRenderer()
        {
        }

        public OrthogonalTiledMapRenderer( TiledMap map )
        {
            SetMap( map );
        }

        public void SetMap( TiledMap map )
        {
            this._tiledMap = map;
        }

        public void RenderTileLayer( MapLayer? gameTilesLayer1 )
        {
        }
    }
}

