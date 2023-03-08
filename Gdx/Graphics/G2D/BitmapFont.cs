namespace LibGDXSharp.G2D
{
    public class BitmapFont
    {
        private const int Log2_Page_Size = 9;
        private const int Page_Size      = 1 << Log2_Page_Size;
        private const int Pages          = 0x10000 / Page_Size;

        BitmapFontData          _data;
        Array< TextureRegion >  _regions;
        private BitmapFontCache _cache;
        private bool            _flipped;
        bool                    _integer;
        private bool            _ownsTexture;

        /**
         * Creates a BitmapFont using the default 15pt Arial font included in the libgdx JAR file.
         * This is convenient to easily
	     * display text without bothering without generating a bitmap font yourself.
         */
        public BitmapFont()
            : this
                (
                 Gdx.Files.Classpath( "com/badlogic/gdx/utils/arial-15.fnt" ),
                 Gdx.Files.Classpath( "com/badlogic/gdx/utils/arial-15.png" ),
                 false,
                 true
                )
        {
        }

        /**
         * Creates a BitmapFont using the default 15pt Arial font included in the libgdx JAR file.
         * This is convenient to easily display text without bothering without generating a
         * bitmap font yourself.
	     * @param flip If true, the glyphs will be flipped for use with a perspective where 0,0 is
         * the upper left corner.
         */
        public BitmapFont( bool flip )
            : this
                (
                 Gdx.Files.Classpath( "com/badlogic/gdx/utils/arial-15.fnt" ),
                 Gdx.Files.Classpath( "com/badlogic/gdx/utils/arial-15.png" ),
                 flip,
                 true
                )
        {
        }

        /** Creates a BitmapFont with the glyphs relative to the specified region. If the region is null, the glyph textures are loaded
	 * from the image file given in the font file. The {@link #dispose()} method will not dispose the region's texture in this
	 * case!
	 * <p>
	 * The font data is not flipped.
	 * @param fontFile the font definition file
	 * @param region The texture region containing the glyphs. The glyphs must be relative to the lower left corner (ie, the region
	 *           should not be flipped). If the region is null the glyph images are loaded from the image path in the font file. */
        public BitmapFont( FileHandle fontFile, TextureRegion region )
            : this( fontFile, region, false )
        {
        }

        /** Creates a BitmapFont with the glyphs relative to the specified region. If the region is null, the glyph textures are loaded
	 * from the image file given in the font file. The {@link #dispose()} method will not dispose the region's texture in this
	 * case!
	 * @param region The texture region containing the glyphs. The glyphs must be relative to the lower left corner (ie, the region
	 *           should not be flipped). If the region is null the glyph images are loaded from the image path in the font file.
	 * @param flip If true, the glyphs will be flipped for use with a perspective where 0,0 is the upper left corner. */
        public BitmapFont( FileHandle fontFile, TextureRegion region, bool flip )
            : this( new BitmapFontData( fontFile, flip ), region, true )
        {
        }

        /** Creates a BitmapFont from a BMFont file. The image file name is read from the BMFont file and the image is loaded from the
	 * same directory. The font data is not flipped. */
        public BitmapFont( FileHandle fontFile )
            : this( fontFile, false )
        {
        }

        /** Creates a BitmapFont from a BMFont file. The image file name is read from the BMFont file and the image is loaded from the
	 * same directory.
	 * @param flip If true, the glyphs will be flipped for use with a perspective where 0,0 is the upper left corner. */
        public BitmapFont( FileHandle fontFile, bool flip )
            : this( new BitmapFontData( fontFile, flip ), ( TextureRegion )null, true )
        {
        }

        /** Creates a BitmapFont from a BMFont file, using the specified image for glyphs. Any image specified in the BMFont file is
	 * ignored.
	 * @param flip If true, the glyphs will be flipped for use with a perspective where 0,0 is the upper left corner. */
        public BitmapFont( FileHandle fontFile, FileHandle imageFile, bool flip )
            : this( fontFile, imageFile, flip, true )
        {
        }

        /** Creates a BitmapFont from a BMFont file, using the specified image for glyphs. Any image specified in the BMFont file is
	 * ignored.
	 * @param flip If true, the glyphs will be flipped for use with a perspective where 0,0 is the upper left corner.
	 * @param integer If true, rendering positions will be at integer values to avoid filtering artifacts. */
        public BitmapFont( FileHandle fontFile, FileHandle imageFile, bool flip, bool integer )
            : this
                (
                 new BitmapFontData( fontFile, flip ),
                 new TextureRegion( new Texture( imageFile, false ) ),
                 integer
                )
        {
            _ownsTexture = true;
        }

        /** Constructs a new BitmapFont from the given {@link BitmapFontData} and {@link TextureRegion}. If the TextureRegion is null,
	 * the image path(s) will be read from the BitmapFontData. The dispose() method will not dispose the texture of the region(s)
	 * if the region is != null.
	 * <p>
	 * Passing a single TextureRegion assumes that your font only needs a single texture page. If you need to support multiple
	 * pages, either let the Font read the images themselves (by specifying null as the TextureRegion), or by specifying each page
	 * manually with the TextureRegion[] constructor.
	 * @param integer If true, rendering positions will be at integer values to avoid filtering artifacts. */
        public BitmapFont( BitmapFontData data, TextureRegion region, bool integer )
            : this( data, region != null ? Array.With( region ) : null, integer )
        {
        }

        /**
         * Constructs a new BitmapFont from the given {@link BitmapFontData} and array of {@link TextureRegion}. If the TextureRegion
	     * is null or empty, the image path(s) will be read from the BitmapFontData. The dispose() method will not dispose the texture
	     * of the region(s) if the regions array is != null and not empty.
	     * @param integer If true, rendering positions will be at integer values to avoid filtering artifacts.
         */
        public BitmapFont( BitmapFontData data, Array< TextureRegion > pageRegions, bool integer )
        {
            this._flipped = data.flipped;
            this._data    = data;
            this._integer = integer;

            if ( pageRegions == null || pageRegions.Size == 0 )
            {
                if ( data.imagePaths == null )
                {
                    throw new ArgumentException
                        ( "If no regions are specified, the font data must have an images path." );
                }

                // Load each path.
                var n = data.imagePaths.Length;

                _regions = new Array<TextureRegion>( capacity: n );

                for ( var i = 0; i < n; i++ )
                {
                    FileHandle file;

                    if ( data.fontFile == null )
                    {
                        file = Gdx.Files.Internal( data.imagePaths[ i ] );
                    }
                    else
                    {
                        file = Gdx.Files.GetFileHandle( data.imagePaths[ i ], data.fontFile.Type );
                    }

                    _regions.Add( new TextureRegion( new Texture( file, false ) ) );
                }

                _ownsTexture = true;
            }
            else
            {
                _regions     = pageRegions;
                _ownsTexture = false;
            }

            _cache = NewFontCache();

            Load( data );
        }

        protected void Load( BitmapFontData data )
        {
            foreach ( Glyph[] page in data.glyphs )
            {
                if ( page == null ) continue;

                foreach ( var glyph in page)
                {
                    if ( glyph != null ) data.setGlyphRegion( glyph, _regions.get( glyph.page ) );
                }
            }

            if ( data.missingGlyph != null ) data.setGlyphRegion( data.missingGlyph, _regions.get( data.missingGlyph.page ) );
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float) */
        public GlyphLayout Draw( Batch batch, CharSequence str, float x, float y )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch, CharSequence str, float x, float y, float targetWidth, int halign, bool wrap )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, targetWidth, halign, wrap );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch,
                                 CharSequence str,
                                 float x,
                                 float y,
                                 int start,
                                 int end,
                                 float targetWidth,
                                 int halign,
                                 bool wrap )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, start, end, targetWidth, halign, wrap );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch,
                                 CharSequence str,
                                 float x,
                                 float y,
                                 int start,
                                 int end,
                                 float targetWidth,
                                 int halign,
                                 bool wrap,
                                 string truncate )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, start, end, targetWidth, halign, wrap, truncate );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public void Draw( Batch batch, GlyphLayout layout, float x, float y )
        {
            _cache.clear();
            _cache.addText( layout, x, y );
            _cache.draw( batch );
        }

        /** Returns the color of text drawn with this font. */
        public Color GetColor()
        {
            return _cache.getColor();
        }

        /** A convenience method for setting the font color. The color can also be set by modifying {@link #getColor()}. */
        public void SetColor( Color color )
        {
            _cache.getColor().set( color );
        }

        /** A convenience method for setting the font color. The color can also be set by modifying {@link #getColor()}. */
        public void SetColor( float r, float g, float b, float a )
        {
            _cache.getColor().set( r, g, b, a );
        }

        public float GetScaleX()
        {
            return _data.scaleX;
        }

        public float GetScaleY()
        {
            return _data.scaleY;
        }

        /** Returns the first texture region. This is included for backwards compatibility, and for convenience since most fonts only
 * use one texture page. For multi-page fonts, use {@link #getRegions()}.
 * @return the first texture region */
        public TextureRegion GetRegion()
        {
            return _regions.first();
        }

        /** Returns the array of TextureRegions that represents each texture page of glyphs.
 * @return the array of texture regions; modifying it may produce undesirable results */
        public Array< TextureRegion > GetRegions()
        {
            return _regions;
        }

        /** Returns the texture page at the given index.
 * @return the texture page at the given index */
        public TextureRegion GetRegion( int index )
        {
            return _regions.get( index );
        }

        /** Returns the line height, which is the distance from one line of text to the next. */
        public float GetLineHeight()
        {
            return _data.lineHeight;
        }

        /** Returns the x-advance of the space character. */
        public float GetSpaceXadvance()
        {
            return _data.spaceXadvance;
        }

        /** Returns the x-height, which is the distance from the top of most lowercase characters to the baseline. */
        public float GetXHeight()
        {
            return _data.xHeight;
        }

        /** Returns the cap height, which is the distance from the top of most uppercase characters to the baseline. Since the drawing
 * position is the cap height of the first line, the cap height can be used to get the location of the baseline. */
        public float GetCapHeight()
        {
            return _data.capHeight;
        }

        /** Returns the ascent, which is the distance from the cap height to the top of the tallest glyph. */
        public float GetAscent()
        {
            return _data.ascent;
        }

        /** Returns the descent, which is the distance from the bottom of the glyph that extends the lowest to the baseline. This
 * number is negative. */
        public float GetDescent()
        {
            return _data.descent;
        }

        /** Returns true if this BitmapFont has been flipped for use with a y-down coordinate system. */
        public bool IsFlipped()
        {
            return _flipped;
        }

        /** Disposes the texture used by this BitmapFont's region IF this BitmapFont created the texture. */
        public void Dispose()
        {
            if ( _ownsTexture )
            {
                for ( var i = 0; i < _regions.size; i++ )
                    _regions.get( i ).getTexture().dispose();
            }
        }

        /** Makes the specified glyphs fixed width. This can be useful to make the numbers in a font fixed width. Eg, when horizontally
 * centering a score or loading percentage text, it will not jump around as different numbers are shown. */
        public void SetFixedWidthGlyphs( CharSequence glyphs )
        {
            var data       = this._data;
            var            maxAdvance = 0;

            for ( int index = 0, end = glyphs.length(); index < end; index++ )
            {
                Glyph g = data.getGlyph( glyphs.charAt( index ) );

                if ( g != null && g.xadvance > maxAdvance )
                {
                    maxAdvance = g.xadvance;
                }
            }

            for ( int index = 0, end = glyphs.length(); index < end; index++ )
            {
                Glyph g = data.getGlyph( glyphs.charAt( index ) );

                if ( g == null ) continue;
                g.xoffset    += ( maxAdvance - g.xadvance ) / 2;
                g.xadvance   =  maxAdvance;
                g.kerning    =  null;
                g.fixedWidth =  true;
            }
        }

        /** Specifies whether to use integer positions. Default is to use them so filtering doesn't kick in as badly. */
        public void SetUseIntegerPositions( bool integer )
        {
            this._integer = integer;
            _cache.SetUseIntegerPositions( integer );
        }

        /** Checks whether this font uses integer positions for drawing. */
        public bool UsesIntegerPositions()
        {
            return _integer;
        }

        /** For expert usage -- returns the BitmapFontCache used by this font, for rendering to a sprite batch. This can be used, for
 * example, to manipulate glyph colors within a specific index.
 * @return the bitmap font cache used by this font */
        public BitmapFontCache GetCache()
        {
            return _cache;
        }

        /** Gets the underlying {@link BitmapFontData} for this BitmapFont. */
        public BitmapFontData GetData()
        {
            return _data;
        }

        /** @return whether the texture is owned by the font, font disposes the texture itself if true */
        public bool OwnsTexture()
        {
            return _ownsTexture;
        }

        /** Sets whether the font owns the texture. In case it does, the font will also dispose of the texture when {@link #dispose()}
 * is called. Use with care!
 * @param ownsTexture whether the font owns the texture */
        public void SetOwnsTexture( bool ownsTexture )
        {
            this._ownsTexture = ownsTexture;
        }

        /** Creates a new BitmapFontCache for this font. Using this method allows the font to provide the BitmapFontCache
 * implementation to customize rendering.
 * <p>
 * Note this method is called by the BitmapFont constructors. If a subclass overrides this method, it will be called before the
 * subclass constructors. */
        public BitmapFontCache NewFontCache()
        {
            return new BitmapFontCache( this, _integer );
        }

        public string ToString()
        {
            return _data.name != null ? _data.name : super.toString();
        }

        /** Represents a single character in a font page. */
        public class Glyph
        {
            public int      id;
            public int      srcX;
            public int      srcY;
            public int      width,   height;
            public float    u,       v, u2, v2;
            public int      xoffset, yoffset;
            public int      xadvance;
            public byte[][] kerning;
            public bool     fixedWidth;

            /** The index to the texture page that holds this glyph. */
            public int page = 0;

            public int GetKerning( char ch )
            {
                if ( kerning != null )
                {
                    var page = kerning[ ch >>> Log2_Page_Size ];

                    if ( page != null ) return page[ ch & Page_Size - 1 ];
                }

                return 0;
            }

            public void SetKerning( int ch, int value )
            {
                if ( kerning == null ) kerning                       = new byte[ Pages ][];
                var page                                          = kerning[ ch >>> Log2_Page_Size ];
                if ( page == null ) kerning[ ch >>> Log2_Page_Size ] = page = new byte[ Page_Size ];
                page[ ch & Page_Size - 1 ] = ( byte )value;
            }

            public string ToString()
            {
                return Character.ToString( ( char )id );
            }
        }

        static int IndexOf( CharSequence text, char ch, int start )
        {
            final int n = text.length();

            for ( ; start < n; start++ )
                if ( text.charAt( start ) == ch )
                    return start;

            return n;
        }

        /** Backing data for a {@link BitmapFont}. */
        public class BitmapFontData
        {
            /** The name of the font, or null. */
            public string name;
            /** An array of the image paths, for multiple texture pages. */
            public string[] imagePaths;
            public FileHandle fontFile;
            public bool       flipped;
            public float      padTop, padRight, padBottom, padLeft;
            /** The distance from one line of text to the next. To set this value, use {@link #setLineHeight(float)}. */
            public float lineHeight;
            /** The distance from the top of most uppercase characters to the baseline. Since the drawing position is the cap height of
		 * the first line, the cap height can be used to get the location of the baseline. */
            public float capHeight = 1;
            /** The distance from the cap height to the top of the tallest glyph. */
            public float ascent;
            /** The distance from the bottom of the glyph that extends the lowest to the baseline. This number is negative. */
            public float descent;
            /** The distance to move down when \n is encountered. */
            public float down;
            /** Multiplier for the line height of blank lines. down * blankLineHeight is used as the distance to move down for a blank
		 * line. */
            public float blankLineScale = 1;
            public float scaleX = 1, scaleY = 1;
            public bool  markupEnabled;
            /** The amount to add to the glyph X position when drawing a cursor between glyphs. This field is not set by the BMFont
		 * file, it needs to be set manually depending on how the glyphs are rendered on the backing textures. */
            public float cursorX;

            public       final glyph[][]
            glyphs = new Glyph[PAGES]
            [];

            /** The glyph to display for characters not in the font. May be null. */
            public Glyph missingGlyph;

            /** The width of the space character. */
            public float spaceXadvance;
            /** The x-height, which is the distance from the top of most lowercase characters to the baseline. */
            public float xHeight = 1;

            /** Additional characters besides whitespace where text is wrapped. Eg, a hypen (-). */
            public char[] breakChars;
            public char[] xChars = { 'x', 'e', 'a', 'o', 'n', 's', 'r', 'c', 'u', 'm', 'v', 'w', 'z' };
            public char[] capChars =
            {
                'M', 'N', 'B', 'D', 'C', 'E', 'F', 'K', 'A', 'G', 'H', 'I', 'J', 'L', 'O', 'P', 'Q', 'R', 'S',
                'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            /** Creates an empty BitmapFontData for configuration before calling {@link #load(FileHandle, bool)}, to subclass, or to
		 * populate yourself, e.g. using stb-truetype or FreeType. */
            public BitmapFontData()
            {
            }

            public BitmapFontData( FileHandle fontFile, bool flip )
            {
                this.fontFile = fontFile;
                this.flipped  = flip;
                
                Load( fontFile, flip );
            }

            public void Load( FileHandle fontFile, bool flip )
            {
                if ( imagePaths != null ) throw new InvalidOperationException( "Already loaded." );

                name = fontFile.NameWithoutExtension();

                BufferedReader reader = new BufferedReader( new InputStreamReader( fontFile.Read() ), 512 );

                try
                {
                    string line = reader.readLine(); // info

                    if ( line == null ) throw new GdxRuntimeException( "File is empty." );

                    line = line.Substring( line.IndexOf( "padding=", StringComparison.Ordinal ) + 8 );
                    var padding = line.Substring( 0, line.IndexOf( ' ' ) ).Split( ",", 4 );

                    if ( padding.Length != 4 ) throw new GdxRuntimeException( "Invalid padding." );

                    padTop    = int.Parse( padding[ 0 ] );
                    padRight  = int.Parse( padding[ 1 ] );
                    padBottom = int.Parse( padding[ 2 ] );
                    padLeft   = int.Parse( padding[ 3 ] );
                    
                    var padY = padTop + padBottom;

                    line = reader.readLine();

                    if ( line == null ) throw new GdxRuntimeException( "Missing common header." );
                    
                    var common = line.Split( " ", 9 ); // At most we want the 6th element; i.e. "page=N"

                    // At least lineHeight and base are required.
                    if ( common.length < 3 ) throw new GdxRuntimeException( "Invalid common header." );

                    if ( !common[ 1 ].startsWith( "lineHeight=" ) ) throw new GdxRuntimeException( "Missing: lineHeight" );
                    lineHeight = Integer.parseInt( common[ 1 ].substring( 11 ) );

                    if ( !common[ 2 ].startsWith( "base=" ) ) throw new GdxRuntimeException( "Missing: base" );
                    float baseLine = Integer.parseInt( common[ 2 ].substring( 5 ) );

                    var pageCount = 1;

                    if ( common.length >= 6 && common[ 5 ] != null && common[ 5 ].startsWith( "pages=" ) )
                    {
                        try
                        {
                            pageCount = Math.max( 1, Integer.parseInt( common[ 5 ].substring( 6 ) ) );
                        }
                        catch ( NumberFormatException ignored )
                        {
                            // Use one page.
                        }
                    }

                    imagePaths = new string[ pageCount ];

                    // Read each page definition.
                    for ( var p = 0; p < pageCount; p++ )
                    {
                        // Read each "page" info line.
                        line = reader.readLine();

                        if ( line == null ) throw new GdxRuntimeException( "Missing additional page definitions." );

                        // Expect ID to mean "index".
                        Matcher matcher = Pattern.compile( ".*id=(\\d+)" ).matcher( line );

                        if ( matcher.find() )
                        {
                            string id = matcher.group( 1 );

                            try
                            {
                                int pageID = Integer.parseInt( id );

                                if ( pageID != p )
                                    throw new GdxRuntimeException( "Page IDs must be indices starting at 0: " + id );
                            }
                            catch ( NumberFormatException ex )
                            {
                                throw new GdxRuntimeException( "Invalid page id: " + id, ex );
                            }
                        }

                        matcher = Pattern.compile( ".*file=\"?([^\"]+)\"?" ).matcher( line );

                        if ( !matcher.find() ) throw new GdxRuntimeException( "Missing: file" );
                        string fileName = matcher.group( 1 );

                        imagePaths[ p ] = fontFile.parent().child( fileName ).path().replaceAll( "\\\\", "/" );
                    }

                    descent = 0;

                    while ( true )
                    {
                        line = reader.readLine();

                        if ( line == null ) break;                   // EOF
                        if ( line.startsWith( "kernings " ) ) break; // Starting kernings block.
                        if ( line.startsWith( "metrics " ) ) break;  // Starting metrics block.

                        if ( !line.startsWith( "char " ) ) continue;

                        var glyph = new Glyph();

                        StringTokenizer tokens = new StringTokenizer( line, " =" );
                        tokens.nextToken();
                        tokens.nextToken();
                        int ch = Integer.parseInt( tokens.nextToken() );

                        if ( ch <= 0 )
                            missingGlyph = glyph;
                        else if ( ch <= Character.MAX_VALUE )
                            setGlyph( ch, glyph );
                        else
                            continue;

                        glyph.id = ch;
                        tokens.nextToken();
                        glyph.srcX = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();
                        glyph.srcY = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();
                        glyph.width = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();
                        glyph.height = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();
                        glyph.xoffset = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();

                        if ( flip )
                            glyph.yoffset = Integer.parseInt( tokens.nextToken() );
                        else
                            glyph.yoffset = -( glyph.height + Integer.parseInt( tokens.nextToken() ) );

                        tokens.nextToken();
                        glyph.xadvance = Integer.parseInt( tokens.nextToken() );

                        // Check for page safely, it could be omitted or invalid.
                        if ( tokens.hasMoreTokens() ) tokens.nextToken();

                        if ( tokens.hasMoreTokens() )
                        {
                            try
                            {
                                glyph.page = Integer.parseInt( tokens.nextToken() );
                            }
                            catch ( NumberFormatException ignored )
                            {
                            }
                        }

                        if ( glyph.width > 0 && glyph.height > 0 ) descent = Math.min( baseLine + glyph.yoffset, descent );
                    }

                    descent += padBottom;

                    while ( true )
                    {
                        line = reader.readLine();

                        if ( line == null ) break;
                        if ( !line.startsWith( "kerning " ) ) break;

                        StringTokenizer tokens = new StringTokenizer( line, " =" );
                        tokens.nextToken();
                        tokens.nextToken();
                        int first = Integer.parseInt( tokens.nextToken() );
                        tokens.nextToken();
                        int second = Integer.parseInt( tokens.nextToken() );

                        if ( first < 0 || first > Character.MAX_VALUE || second < 0 || second > Character.MAX_VALUE ) continue;
                        Glyph glyph = getGlyph( ( char )first );
                        tokens.nextToken();
                        int amount = Integer.parseInt( tokens.nextToken() );

                        if ( glyph != null )
                        {
                            // Kernings may exist for glyph pairs not contained in the font.
                            glyph.setKerning( second, amount );
                        }
                    }

                    var  hasMetricsOverride    = false;
                    float overrideAscent        = 0;
                    float overrideDescent       = 0;
                    float overrideDown          = 0;
                    float overrideCapHeight     = 0;
                    float overrideLineHeight    = 0;
                    float overrideSpaceXAdvance = 0;
                    float overrideXHeight       = 0;

                    // Metrics override
                    if ( line != null && line.startsWith( "metrics " ) )
                    {

                        hasMetricsOverride = true;

                        StringTokenizer tokens = new StringTokenizer( line, " =" );
                        tokens.nextToken();
                        tokens.nextToken();
                        overrideAscent = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideDescent = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideDown = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideCapHeight = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideLineHeight = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideSpaceXAdvance = Float.parseFloat( tokens.nextToken() );
                        tokens.nextToken();
                        overrideXHeight = Float.parseFloat( tokens.nextToken() );
                    }

                    Glyph spaceGlyph = getGlyph( ' ' );

                    if ( spaceGlyph == null )
                    {
                        spaceGlyph    = new Glyph();
                        spaceGlyph.id = ' ';
                        Glyph xadvanceGlyph                        = getGlyph( 'l' );
                        if ( xadvanceGlyph == null ) xadvanceGlyph = getFirstGlyph();
                        spaceGlyph.xadvance = xadvanceGlyph.xadvance;
                        setGlyph( ' ', spaceGlyph );
                    }

                    if ( spaceGlyph.width == 0 )
                    {
                        spaceGlyph.width   = ( int )( padLeft + spaceGlyph.xadvance + padRight );
                        spaceGlyph.xoffset = ( int )-padLeft;
                    }

                    spaceXadvance = spaceGlyph.xadvance;

                    Glyph xGlyph = null;
                    for ( char xChar :
                    xChars) {
                        xGlyph = getGlyph( xChar );

                        if ( xGlyph != null ) break;
                    }

                    if ( xGlyph == null ) xGlyph = getFirstGlyph();
                    xHeight = xGlyph.height - padY;

                    Glyph capGlyph = null;
                    for ( char capChar :
                    capChars) {
                        capGlyph = getGlyph( capChar );

                        if ( capGlyph != null ) break;
                    }

                    if ( capGlyph == null )
                    {
                        for ( glyph[] page :
                        this.glyphs) {
                            if ( page == null ) continue;
                            for ( glyph glyph :
                            page) {
                                if ( glyph == null || glyph.height == 0 || glyph.width == 0 ) continue;
                                capHeight = Math.max( capHeight, glyph.height );
                            }
                        }
                    }
                    else
                        capHeight = capGlyph.height;

                    capHeight -= padY;

                    ascent = baseLine - capHeight;
                    down   = -lineHeight;

                    if ( flip )
                    {
                        ascent = -ascent;
                        down   = -down;
                    }

                    if ( hasMetricsOverride )
                    {
                        this.ascent        = overrideAscent;
                        this.descent       = overrideDescent;
                        this.down          = overrideDown;
                        this.capHeight     = overrideCapHeight;
                        this.lineHeight    = overrideLineHeight;
                        this.spaceXadvance = overrideSpaceXAdvance;
                        this.xHeight       = overrideXHeight;
                    }

                }
                catch ( Exception ex )
                {
                    throw new GdxRuntimeException( "Error loading font file: " + fontFile, ex );
                }
                finally
                {
                    StreamUtils.CloseQuietly( reader );
                }
            }

            public void SetGlyphRegion( Glyph glyph, TextureRegion region )
            {
                Texture texture      = region.GetTexture();
                var   invTexWidth  = 1.0f / texture.GetWidth();
                var   invTexHeight = 1.0f / texture.GetHeight();

                float                  offsetX      = 0, offsetY = 0;
                float                  u            = region.u;
                float                  v            = region.v;
                float                  regionWidth  = region.GetRegionWidth();
                float                  regionHeight = region.GetRegionHeight();
                
                if ( region is AtlasRegion)
                {
                    // Compensate for whitespace stripped from left and top edges.
                    AtlasRegion atlasRegion = ( AtlasRegion )region;
                    offsetX = atlasRegion.offsetX;
                    offsetY = atlasRegion.originalHeight - atlasRegion.packedHeight - atlasRegion.offsetY;
                }

                float x  = glyph.srcX;
                float x2 = glyph.srcX + glyph.width;
                float y  = glyph.srcY;
                float y2 = glyph.srcY + glyph.height;

                // Shift glyph for left and top edge stripped whitespace. Clip glyph for right and bottom edge stripped whitespace.
                // Note if the font region has padding, whitespace stripping must not be used.
                if ( offsetX > 0 )
                {
                    x -= offsetX;

                    if ( x < 0 )
                    {
                        glyph.width   += x;
                        glyph.xoffset -= x;
                        x             =  0;
                    }

                    x2 -= offsetX;

                    if ( x2 > regionWidth )
                    {
                        glyph.width -= x2 - regionWidth;
                        x2          =  regionWidth;
                    }
                }

                if ( offsetY > 0 )
                {
                    y -= offsetY;

                    if ( y < 0 )
                    {
                        glyph.height += y;
                        if ( glyph.height < 0 ) glyph.height = 0;
                        y = 0;
                    }

                    y2 -= offsetY;

                    if ( y2 > regionHeight )
                    {
                        var amount = y2 - regionHeight;
                        glyph.height  -= amount;
                        glyph.yoffset += amount;
                        y2            =  regionHeight;
                    }
                }

                glyph.u  = u + x * invTexWidth;
                glyph.u2 = u + x2 * invTexWidth;

                if ( flipped )
                {
                    glyph.v  = v + y * invTexHeight;
                    glyph.v2 = v + y2 * invTexHeight;
                }
                else
                {
                    glyph.v2 = v + y * invTexHeight;
                    glyph.v  = v + y2 * invTexHeight;
                }
            }

            /** Sets the line height, which is the distance from one line of text to the next. */
            public void SetLineHeight( float height )
            {
                lineHeight = height * scaleY;
                down       = flipped ? lineHeight : -lineHeight;
            }

            public void SetGlyph( int ch, Glyph glyph )
            {
                Glyph[] page                                 = glyphs[ ch / Page_Size ];
                if ( page == null ) glyphs[ ch / Page_Size ] = page = new Glyph[ Page_Size ];
                page[ ch & Page_Size - 1 ] = glyph;
            }

            public Glyph GetFirstGlyph()
            {
                foreach ( Glyph[] page in this.glyphs)
                {
                    if ( page == null ) continue;
                 
                    foreach ( var glyph in page)
                    {
                        if ( glyph == null || glyph.height == 0 || glyph.width == 0 ) continue;

                        return glyph;
                    }
                }

                throw new GdxRuntimeException( "No glyphs found." );
            }

            /** Returns true if the font has the glyph, or if the font has a {@link #missingGlyph}. */
            public bool HasGlyph( char ch )
            {
                if ( missingGlyph != null ) return true;

                return getGlyph( ch ) != null;
            }

            /** Returns the glyph for the specified character, or null if no such glyph exists. Note that
		 * {@link #getGlyphs(GlyphRun, CharSequence, int, int, Glyph)} should be be used to shape a string of characters into a list
		 * of glyphs. */
            public Glyph GetGlyph( char ch )
            {
                Glyph[] page = glyphs[ ch / Page_Size ];

                if ( page != null ) return page[ ch & Page_Size - 1 ];

                return null;
            }

            /** Using the specified string, populates the glyphs and positions of the specified glyph run.
		 * @param str Characters to convert to glyphs. Will not contain newline or color tags. May contain "[[" for an escaped left
		 *           square bracket.
		 * @param lastGlyph The glyph immediately before this run, or null if this is run is the first on a line of text. */
            public void GetGlyphs( GlyphRun run, CharSequence str, int start, int end, Glyph lastGlyph )
            {
                var max = end - start;

                if ( max == 0 ) return;
                var           markupEnabled = this.markupEnabled;
                var          scaleX        = this.scaleX;
                Array< Glyph > glyphs        = run.glyphs;
                FloatArray     xAdvances     = run.xAdvances;

                // Guess at number of glyphs needed.
                glyphs.ensureCapacity( max );
                run.xAdvances.ensureCapacity( max + 1 );

                do
                {
                    char ch = str.charAt( start++ );

                    if ( ch == '\r' ) continue; // Ignore.
                    Glyph glyph = getGlyph( ch );

                    if ( glyph == null )
                    {
                        if ( missingGlyph == null ) continue;
                        glyph = missingGlyph;
                    }

                    glyphs.add( glyph );

                    xAdvances.add
                        (
                         lastGlyph == null // First glyph on line, adjust the position so it isn't drawn left of 0.
                             ? ( glyph.fixedWidth ? 0 : -glyph.xoffset * scaleX - padLeft )
                             : ( lastGlyph.xadvance + lastGlyph.getKerning( ch ) ) * scaleX
                        );

                    lastGlyph = glyph;

                    // "[[" is an escaped left square bracket, skip second character.
                    if ( markupEnabled && ch == '[' && start < end && str.charAt( start ) == '[' ) start++;
                }
                while ( start < end );

                if ( lastGlyph != null )
                {
                    var lastGlyphWidth = lastGlyph.fixedWidth
                        ? lastGlyph.xadvance * scaleX
                        : ( lastGlyph.width + lastGlyph.xoffset ) * scaleX - padRight;

                    xAdvances.Add( lastGlyphWidth );
                }
            }

            /** Returns the first valid glyph index to use to wrap to the next line, starting at the specified start index and
		 * (typically) moving toward the beginning of the glyphs array. */
            public int GetWrapIndex( Array< Glyph > glyphs, int start )
            {
                var      i           = start - 1;
                Object[] glyphsItems = glyphs.items;
                var     ch          = ( char )( ( Glyph )glyphsItems[ i ] ).id;

                if ( IsWhitespace( ch ) ) return i;
                if ( IsBreakChar( ch ) ) i--;

                for ( ; i > 0; i-- )
                {
                    ch = ( char )( ( Glyph )glyphsItems[ i ] ).id;

                    if ( IsWhitespace( ch ) || IsBreakChar( ch ) ) return i + 1;
                }

                return 0;
            }

            public bool IsBreakChar( char c )
            {
                if ( breakChars == null ) return false;

                foreach ( char br in breakChars )
                {
                    if ( c == br ) return true;
                }

                return false;
            }

            public bool IsWhitespace( char c )
            {
                switch ( c )
                {
                    case '\n':
                    case '\r':
                    case '\t':
                    case ' ':
                        return true;

                    default:
                        return false;
                }
            }

            /** Returns the image path for the texture page at the given index (the "id" in the BMFont file). */
            public string GetImagePath( int index )
            {
                return imagePaths[ index ];
            }

            public string[] GetImagePaths()
            {
                return imagePaths;
            }

            public FileHandle GetFontFile()
            {
                return fontFile;
            }

            /**
             * Scales the font by the specified amounts on both axes
		     * <p>
		     * Note that smoother scaling can be achieved if the texture backing the BitmapFont is using {@link TextureFilter#Linear}.
		     * The default is Nearest, so use a BitmapFont constructor that takes a {@link TextureRegion}.
		     * @throws IllegalArgumentException if scaleX or scaleY is zero.
             */
            public void SetScale( float scaleX, float scaleY )
            {
                if ( scaleX == 0 ) throw new ArgumentException( "scaleX cannot be 0." );
                if ( scaleY == 0 ) throw new ArgumentException( "scaleY cannot be 0." );
                
                var x = scaleX / this.scaleX;
                var y = scaleY / this.scaleY;
                
                lineHeight    *= y;
                spaceXadvance *= x;
                xHeight       *= y;
                capHeight     *= y;
                ascent        *= y;
                descent       *= y;
                down          *= y;
                padLeft       *= x;
                padRight      *= x;
                padTop        *= y;
                padBottom     *= y;
                this.scaleX   =  scaleX;
                this.scaleY   =  scaleY;
            }

            /**
             * Scales the font by the specified amount in both directions.
		     * @see #setScale(float, float)
		     * @throws IllegalArgumentException if scaleX or scaleY is zero.
             */
            public void SetScale( float scaleXy )
            {
                SetScale( scaleXy, scaleXy );
            }

            /**
             * Sets the font's scale relative to the current scale.
		     * @see #setScale(float, float)
		     * @throws IllegalArgumentException if the resulting scale is zero.
             */
            public void Scale( float amount )
            {
                SetScale( scaleX + amount, scaleY + amount );
            }

            public override string ToString()
            {
                return name ?? base.ToString();
            }
        }

    }
}
