namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// <seealso cref="AssetLoader{T,TP}"/> for <seealso cref="Texture"/> instances.
    /// The pixel data is loaded asynchronously. The texture is then created on the
    /// rendering thread, synchronously. Passing a <seealso cref="TextureParameter"/>
    /// to <seealso cref="AssetManager"/>.Load() allows one to specify parameters as
    /// can be passed to the various Texture constructors, e.g. filtering, whether
    /// to generate mipmaps and so on.
    /// </summary>
    public class TextureLoader : AsynchronousAssetLoader< Texture, TextureLoader.TextureParameter >
    {
        public class TextureLoaderInfo
        {
            public string?      Filename { get; set; }
            public ITextureData? Data     { get; set; }
            public Texture?     Texture  { get; set; }
        }

        public TextureLoaderInfo Info { get; set; } = new TextureLoaderInfo();

        public TextureLoader( IFileHandleResolver resolver ) : base( resolver )
        {
        }

        public override void LoadAsync( AssetManager manager,
                                        string fileName,
                                        FileHandle file,
                                        TextureParameter? parameter )
        {
            Info.Filename = fileName;

            if ( parameter == null || parameter.TextureData == null )
            {
                Pixmap.Format? format    = null;
                var           genMipMaps = false;

                Info.Texture = null;

                if ( parameter != null )
                {
                    format       = parameter.Format;
                    genMipMaps   = parameter.GenMipMaps;
                    Info.Texture = parameter.Texture;
                }

                Info.Data = ITextureData.Factory.LoadFromFile( file, format, genMipMaps );
            }
            else
            {
                Info.Data    = parameter.TextureData;
                Info.Texture = parameter.Texture;
            }

            if ( !Info.Data.isPrepared() )
            {
                Info.Data.prepare();
            }
        }

        public override Texture LoadSync( AssetManager manager,
                                          string fileName,
                                          FileHandle file,
                                          TextureParameter? parameter )
        {
            if ( Info == null )
            {
                return null;
            }

            Texture texture = Info.Texture;

            if ( texture != null )
            {
                texture.load( Info.Data );
            }
            else
            {
                texture = new Texture( Info.Data );
            }

            if ( parameter != null )
            {
                texture.SetFilter( parameter.MinFilter, parameter.MagFilter );
                texture.SetWrap( parameter.WrapU, parameter.WrapV );
            }

            return texture;
        }

        public override Array< AssetDescriptor >? GetDependencies( string fileName,
                                                                   FileHandle file,
                                                                   TextureParameter parameter )
        {
            return null;
        }

        public class TextureParameter : AssetLoaderParameters< Texture >
        {
            /// <summary>
            /// the format of the final Texture. Uses the source images format if null
            /// </summary>
            public Pixmap.Format? Format { get; set; } = null;

            /// <summary>
            /// whether to generate mipmaps
            /// </summary>
            public bool GenMipMaps { get; set; } = false;

            /// <summary>
            /// The texture to put the <seealso cref="TextureData"/> in, optional.
            /// </summary>
            public Texture? Texture { get; set; } = null;

            /// <summary>
            /// TextureData for textures created on the fly, optional. When set, all format and genMipMaps are ignored
            /// </summary>
            public ITextureData? TextureData { get; set; } = null;

            public TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;
            public TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;
            public TextureWrap   WrapU     { get; set; } = TextureWrap.ClampToEdge;
            public TextureWrap   WrapV     { get; set; } = TextureWrap.ClampToEdge;
        }
    }
}
