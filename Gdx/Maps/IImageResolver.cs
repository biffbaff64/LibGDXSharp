using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps
{
    public interface IImageResolver
    {
        public TextureRegion GetImage( string name );

        public class DirectImageResolver : IImageResolver
        {
            private readonly ObjectMap< string, Texture > _images;

            public DirectImageResolver( ObjectMap< string, Texture > images )
            {
                this._images = images;
            }

            public TextureRegion GetImage( string name )
            {
                return new TextureRegion( _images[ name ] );
            }
        }

        public class AssetManagerImageResolver : IImageResolver
        {
            private readonly AssetManager _assetManager;

            public AssetManagerImageResolver( AssetManager assetManager )
            {
                this._assetManager = assetManager;
            }

            public TextureRegion GetImage( string name )
            {
                return new TextureRegion( _assetManager.Get( name, typeof(Texture)));
            }
        }

        public class TextureAtlasImageResolver : IImageResolver
        {
            private readonly TextureAtlas _atlas;

            public TextureAtlasImageResolver( TextureAtlas atlas )
            {
                this._atlas = atlas;
            }

            public TextureRegion GetImage( string name )
            {
                return _atlas.FindRegion( name );
            }
        }
    }

}
