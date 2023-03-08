using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled
{
    public class TmxMapLoader : BaseTmxMapLoader< TmxMapLoader.Parameters >
    {
        public class Parameters : BaseTmxMapLoader.Parameters
        {
        }

        public TmxMapLoader() : base( new InternalFileHandleResolver() )
        {
        }

        /**
     * Creates loader
	 * @param resolver
     */
        public TmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
        {
        }

        /**
     * Loads the {@link TiledMap} from the given file. The file is resolved via the {@link FileHandleResolver} set in the
	 * constructor of this class. By default it will resolve to an internal file. The map will be loaded for a y-up coordinate
	 * system.
	 * @param fileName the filename
	 * @return the TiledMap
     */
        public TiledMap Load( string fileName )
        {
            return Load( fileName, new TmxMapLoader.Parameters() );
        }

        /** Loads the {@link TiledMap} from the given file. The file is resolved via the {@link FileHandleResolver} set in the
	 * constructor of this class. By default it will resolve to an internal file.
	 * @param fileName the filename
	 * @param parameter specifies whether to use y-up, generate mip maps etc.
	 * @return the TiledMap */
        public TiledMap Load( string fileName, TmxMapLoader.Parameters parameter )
        {
            FileHandle tmxFile = resolve( fileName );

            this.root = xml.parse( tmxFile );

            var textures = new Dictionary< string, Texture >();

            var textureFiles = GetDependencyFileHandles( tmxFile );

            foreach ( FileHandle textureFile in textureFiles )
            {
                var texture = new Texture( textureFile, parameter.GenerateMipMaps );

                texture.SetFilter( parameter.TextureMinFilter, parameter.TextureMagFilter );
                textures.Put( textureFile.Path(), texture );
            }

            var map = LoadTiledMap( tmxFile, parameter, new IImageResolver.DirectImageResolver( textures ) );

            map.setOwnedResources( textures.Values().toArray() );

            return map;
        }

        public void LoadAsync( AssetManager manager, string fileName, FileHandle tmxFile, Parameters parameter )
        {
            this.Map = LoadTiledMap( tmxFile, parameter, new IImageResolver.AssetManagerImageResolver( manager ) );
        }

        public TiledMap LoadSync( AssetManager manager, string fileName, FileHandle file, Parameters parameter )
        {
            return Map;
        }

        protected List< AssetDescriptor > GetDependencyAssetDescriptors( FileHandle tmxFile, TextureLoader.TextureParameter textureParameter )
        {
            List< AssetDescriptor > descriptors = new List< AssetDescriptor >();

            List<TFileHandle> fileHandles = GetDependencyFileHandles( tmxFile );

            foreach ( FileHandle handle in fileHandles)
            {
                descriptors.add( new AssetDescriptor( handle, Texture.class, textureParameter));
            }

            return descriptors;
        }

        protected List< FileHandle > GetDependencyFileHandles( FileHandle tmxFile )
        {
            List< FileHandle > fileHandles = new List< FileHandle >();

            // TileSet descriptors
            foreach ( Element tileset in root.getChildrenByName( "tileset" ) )
            {
                string source = tileset.getAttribute( "source", null );

                if ( source != null )
                {
                    FileHandle tsxFile = GetRelativeFileHandle( tmxFile, source );
                    tileset = xml.parse( tsxFile );
                    Element imageElement = tileset.getChildByName( "image" );

                    if ( imageElement != null )
                    {
                        string     imageSource = tileset.getChildByName( "image" ).getAttribute( "source" );
                        FileHandle image       = GetRelativeFileHandle( tsxFile, imageSource );
                        fileHandles.add( image );
                    }
                    else
                    {
                        foreach ( Element tile in tileset.getChildrenByName( "tile" ) )
                        {
                            string     imageSource = tile.getChildByName( "image" ).getAttribute( "source" );
                            FileHandle image       = GetRelativeFileHandle( tsxFile, imageSource );
                            fileHandles.add( image );
                        }
                    }
                }
                else
                {
                    Element imageElement = tileset.getChildByName( "image" );

                    if ( imageElement != null )
                    {
                        string     imageSource = tileset.getChildByName( "image" ).getAttribute( "source" );
                        FileHandle image       = GetRelativeFileHandle( tmxFile, imageSource );
                        fileHandles.add( image );
                    }
                    else
                    {
                        foreach ( Element tile in tileset.getChildrenByName( "tile" ) )
                        {
                            string     imageSource = tile.getChildByName( "image" ).getAttribute( "source" );
                            FileHandle image       = GetRelativeFileHandle( tmxFile, imageSource );
                            fileHandles.add( image );
                        }
                    }
                }
            }

            // ImageLayer descriptors
            foreach ( Element imageLayer in root.getChildrenByName( "imagelayer" ) )
            {
                Element image  = imageLayer.getChildByName( "image" );
                string  source = image.getAttribute( "source", null );

                if ( source != null )
                {
                    FileHandle handle = GetRelativeFileHandle( tmxFile, source );
                    fileHandles.add( handle );
                }
            }

            return fileHandles;
        }

        protected void AddStaticTiles( FileHandle tmxFile,
                                       ImageResolver imageResolver,
                                       TiledMapTileSet tileSet,
                                       Element element,
                                       List< Element > tileElements,
                                       string name,
                                       int firstgid,
                                       int tilewidth,
                                       int tileheight,
                                       int spacing,
                                       int margin,
                                       string source,
                                       int offsetX,
                                       int offsetY,
                                       string imageSource,
                                       int imageWidth,
                                       int imageHeight,
                                       FileHandle image )
        {

            MapProperties props = tileSet.GetProperties();

            if ( image != null )
            {
                // One image for the whole tileSet
                TextureRegion texture = imageResolver.getImage( image.path() );

                props.put( "imagesource", imageSource );
                props.put( "imagewidth", imageWidth );
                props.put( "imageheight", imageHeight );
                props.put( "tilewidth", tilewidth );
                props.put( "tileheight", tileheight );
                props.put( "margin", margin );
                props.put( "spacing", spacing );

                int stopWidth  = texture.getRegionWidth() - tilewidth;
                int stopHeight = texture.getRegionHeight() - tileheight;

                int id = firstgid;

                for ( int y = margin; y <= stopHeight; y += tileheight + spacing )
                {
                    for ( int x = margin; x <= stopWidth; x += tilewidth + spacing )
                    {
                        TextureRegion tileRegion = new TextureRegion( texture, x, y, tilewidth, tileheight );
                        int           tileId     = id++;
                        AddStaticTiledMapTile( tileSet, tileRegion, tileId, offsetX, offsetY );
                    }
                }
            }
            else
            {
                // Every tile has its own image source
                foreach ( Element tileElement in tileElements )
                {
                    Element imageElement = tileElement.getChildByName( "image" );

                    if ( imageElement != null )
                    {
                        imageSource = imageElement.getAttribute( "source" );

                        if ( source != null )
                        {
                            image = GetRelativeFileHandle( GetRelativeFileHandle( tmxFile, source ), imageSource );
                        }
                        else
                        {
                            image = GetRelativeFileHandle( tmxFile, imageSource );
                        }
                    }

                    var texture = imageResolver.GetImage( image.Path() );
                    var tileId  = firstgid + tileElement.getIntAttribute( "id" );

                    AddStaticTiledMapTile( tileSet, texture, tileId, offsetX, offsetY );
                }
            }
        }
    }
