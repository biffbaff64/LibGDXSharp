using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// 
    /// </summary>
    public class BitmapFontLoader : AsynchronousAssetLoader< BitmapFont, BitmapFontLoader.BitmapFontParameter >
    {
        private BitmapFont.BitmapFontData _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        public BitmapFontLoader( IFileHandleResolver resolver ) : base( resolver )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override Array< AssetDescriptor< BitmapFont > > GetDependencies( string fileName,
                                                                                FileHandle file,
                                                                                BitmapFontParameter parameter )
        {
            var deps = new Array< AssetDescriptor< BitmapFont > >();

            if ( parameter != null && parameter.BitmapFontData != null )
            {
                _data = parameter.BitmapFontData;

                return deps;
            }

            _data = new BitmapFont.BitmapFontData( file, parameter != null && parameter.Flip );

            if ( parameter != null && parameter.AtlasName != null )
            {
                deps.Add( new AssetDescriptor< BitmapFont >( parameter.AtlasName, typeof(TextureAtlas) ) );
            }
            else
            {
                for ( var i = 0; i < _data.GetImagePaths().Length; i++ )
                {
                    var path     = _data.GetImagePath( i );
                    var resolved = Resolve( path );

                    TextureLoader.TextureParameter textureParams = new TextureLoader.TextureParameter();

                    if ( parameter != null )
                    {
                        textureParams.GenMipMaps = parameter.GenMipMaps;
                        textureParams.MinFilter  = parameter.MinFilter;
                        textureParams.MagFilter  = parameter.MagFilter;
                    }

                    var descriptor = new AssetDescriptor< BitmapFont >( resolved, typeof(Texture), textureParams );
                    deps.Add( descriptor );
                }
            }

            return deps;
        }

        public class BitmapFontParameter : AssetLoaderParameters< BitmapFont >
        {
            /// Flips the font vertically if <code>true</code>. Defaults to <code>false</code>.
            public bool Flip { get; set; } = false;

            /// Generates mipmaps for the font if <code>true</code>. Defaults to <code>false</code>.
            public bool GenMipMaps { get; set; } = false;

            /// The <see cref="TextureFilter"/> to use when scaling down the <see cref="BitmapFont"/>.
            /// Defaults to <see cref="TextureFilter#Nearest"/>.
            public TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;

            /// The <see cref="TextureFilter"/> to use when scaling up the <see cref="BitmapFont"/>.
            /// Defaults to <see cref="TextureFilter#Nearest"/>.
            public TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;

            /// optional <see cref="BitmapFont.BitmapFontData"/> to be used instead of loading the <see cref="Texture"/>
            /// directly. Use this if your font is embedded in a <see cref="Skin"/>.
            public BitmapFont.BitmapFontData BitmapFontData { get; set; } = null;

            /// The name of the <see cref="TextureAtlas"/> to load the <see cref="BitmapFont"/> Optional;
            /// if null, will look for a separate image.
            public string AtlasName { get; set; } = null;
        }

        public override void LoadAsync( AssetManager manager,
                                        string fileName,
                                        FileHandle file,
                                        BitmapFontParameter parameter )
        {
        }

        public override BitmapFont LoadSync( AssetManager manager,
                                             string fileName,
                                             FileHandle file,
                                             BitmapFontParameter? parameter )
        {
            if ( parameter != null && parameter.AtlasName != null )
            {
                var atlas  = manager.Get( parameter.AtlasName, typeof(TextureAtlas) );
                var name   = file.Sibling( _data.imagePaths[ 0 ] ).nameWithoutExtension().toString();
                var region = atlas.FindRegion( name );

                if ( region == null )
                {
                    throw new GdxRuntimeException
                        (
                         "Could not find font region " + name + " in atlas " + parameter.AtlasName
                        );
                }

                return new BitmapFont( file, region );
            }
            else
            {
                var n    = _data.GetImagePaths().Length;
                var regs = new Array<TextureRegion>( capacity: n );

                for ( var i = 0; i < n; i++ )
                {
                    regs.Add( new TextureRegion( manager.Get( _data.GetImagePath( i ), typeof(Texture))));
                }

                return new BitmapFont( _data, regs, true );
            }
        }
    }
}
