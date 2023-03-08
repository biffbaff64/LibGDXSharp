using IDisposable = System.IDisposable;

namespace LibGDXSharp.Maps
{
    public class Map : IDisposable
    {
        private readonly MapLayers     _layers     = new MapLayers();
        private readonly MapProperties _properties = new MapProperties();

        public Map()
        {
        }

        public MapLayers GetLayers()
        {
            return _layers;
        }

        public MapProperties GetProperties()
        {
            return _properties;
        }

        public void Dispose()
        {
        }
    }
}
