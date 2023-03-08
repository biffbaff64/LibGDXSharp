namespace LibGDXSharp.Graphics
{
    /// <summary>
    /// Used by a Texture to load the pixel data. A TextureData can either return
    /// a Pixmap or upload the pixel data itself. It signals it's type via getType()
    /// to the Texture that's using it. The Texture will then either invoke
    /// consumePixmap() or consumeCustomData(int). These are the first methods to be
    /// called by Texture. After that the Texture will invoke the other methods to
    /// find out about the size of the image data, the format, whether mipmaps should
    /// be generated and whether the TextureData is able to manage the pixel data if
    /// the OpenGL ES context is lost. In case the TextureData implementation has the
    /// type TextureData.TextureDataType.Custom, the implementation has to generate
    /// the mipmaps itself if necessary. See MipMapGenerator. Before a call to either
    /// consumePixmap() or consumeCustomData(int), Texture will bind the OpenGL ES
    /// texture. Look at FileTextureData and ETC1TextureData for example
    /// implementations of this interface.
    /// </summary>
    public interface ITextureData
    {

        public static class Factory
        {
            public static ITextureData LoadFromFile( FileHandle file, bool useMipMaps )
            {
                return LoadFromFile( file, null, useMipMaps );
            }

            public static ITextureData? LoadFromFile( FileHandle? file, Pixmap.Format? format, bool useMipMaps )
            {
                if ( file == null ) return null;

                if ( file.Name().EndsWith( ".cim" ) )
                {
                    return new FileTextureData( file, PixmapIO.readCIM( file ), format, useMipMaps );
                }

                if ( file.Name().EndsWith( ".etc1" ) )
                {
                    return new ETC1TextureData( file, useMipMaps );
                }
                
                if ( file.Name().EndsWith( ".ktx" ) || file.Name().EndsWith( ".zktx" ) ) return new KTXTextureData( file, useMipMaps );

                return new FileTextureData( file, new Pixmap( file ), format, useMipMaps );
            }

        }
    }
}
