namespace LibGDXSharp.Maps.Tiled
{
    public class TiledMapTileSets : IEnumerable< TiledMapTileSet >
    {
        private readonly List< TiledMapTileSet > _tilesets;

        /**
         * Creates an empty collection of tilesets.
         */
        public TiledMapTileSets()
        {
            _tilesets = new List< TiledMapTileSet >();
        }

        /**
         * @param index index to get the desired {@link TiledMapTileSet} at.
    	 * @return tileset at index
         */
        public TiledMapTileSet GetTileSet( int index )
        {
            return _tilesets[ index ];
        }

        /**
         * @param name Name of the {@link TiledMapTileSet} to retrieve.
    	 * @return tileset with matching name, null if it doesn't exist
         */
        public TiledMapTileSet? GetTileSet( string name )
        {
            foreach ( var tileset in _tilesets)
            {
                if ( name.Equals( tileset.Name ) )
                {
                    return tileset;
                }
            }

            return null;
        }

        /**
         * @param tileset set to be added to the collection
         */
        public void AddTileSet( TiledMapTileSet tileset )
        {
            _tilesets.Add( tileset );
        }

        /**
         * Removes tileset at index
    	 * @param index index at which to remove a tileset.
         */
        public void RemoveTileSet( int index )
        {
            _tilesets.RemoveAt( index );
        }

        /**
         * @param tileset set to be removed
         */
        public void RemoveTileSet( TiledMapTileSet tileset )
        {
            _tilesets.Remove( tileset );
        }

        /**
         * @param id id of the {@link TiledMapTile} to get.
	     * @return tile with matching id, null if it doesn't exist
         */
        public TiledMapTile? GetTile( int id )
        {
            // The purpose of backward iteration here is to maintain backwards compatibility
            // with maps created with earlier versions of a shared tileset.  The assumption
            // is that the tilesets are in order of ascending firstgid, and by backward
            // iterating precedence for conflicts is given to later tilesets in the list, 
            // which are likely to be the earlier version of any given gid.  
            // See TiledMapModifiedExternalTilesetTest for example of this issue.
            for ( var i = _tilesets.Count - 1; i >= 0; i-- )
            {
                TiledMapTileSet tileset = _tilesets[ i ];
                TiledMapTile    tile    = tileset.GetTile( id );

                if ( tile != null )
                {
                    return tile;
                }
            }

            return null;
        }

        public IEnumerator< TiledMapTileSet > GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

