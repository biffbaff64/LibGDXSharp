namespace LibGDXSharp.Maps
{
    public class MapGroupLayer : MapLayer
    {
        private MapLayers _layers = new MapLayers();

        public MapLayers GetLayers()
        {
            return _layers;
        }

        public new void InvalidateRenderOffset()
        {
            base.InvalidateRenderOffset();

            for ( var i = 0; i < _layers.Size(); i++ )
            {
                var child = _layers.Get( i );
                child.InvalidateRenderOffset();
            }
        }
    }
}
