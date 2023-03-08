using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled
{
    public interface ITiledMapTile
    {
        public enum BlendMode
        {
            None,
            Alpha
        }

        public int GetId();

        public void SetId( int id );

        /** @return the {@link BlendMode} to use for rendering the tile */
        public BlendMode GetBlendMode();

        /** Sets the {@link BlendMode} to use for rendering the tile
	 * 
	 * @param blendMode the blend mode to use for rendering the tile */
        public void SetBlendMode( BlendMode blendMode );

        /** @return texture region used to render the tile */
        public TextureRegion GetTextureRegion();

        /** Sets the texture region used to render the tile */
        public void SetTextureRegion( TextureRegion textureRegion );

        /** @return the amount to offset the x position when rendering the tile */
        public float GetOffsetX();

        /** Set the amount to offset the x position when rendering the tile */
        public void SetOffsetX( float offsetX );

        /** @return the amount to offset the y position when rendering the tile */
        public float GetOffsetY();

        /** Set the amount to offset the y position when rendering the tile */
        public void SetOffsetY( float offsetY );

        /** @return tile's properties set */
        public MapProperties GetProperties();

        /** @return collection of objects contained in the tile */
        public MapObjects GetObjects();
    }
}

