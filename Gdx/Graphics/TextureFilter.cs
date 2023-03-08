namespace LibGDXSharp.Graphics
{
    public sealed class TextureFilter
    {
        /// <summary>
        /// Fetch the nearest texel that best maps to the pixel on screen. </summary>
        public readonly static TextureFilter Nearest = new TextureFilter( "Nearest", InnerEnum.Nearest, IGL20.GL_Nearest );

        /// <summary>
        /// Fetch four nearest texels that best maps to the pixel on screen. </summary>
        public readonly static TextureFilter Linear = new TextureFilter( "Linear", InnerEnum.Linear, IGL20.GL_Linear );

        /// <seealso cref="TextureFilter.MipMapLinearLinear "/>
        public readonly static TextureFilter MipMap = new TextureFilter( "MipMap", InnerEnum.MipMap, IGL20.GL_Linear_Mipmap_Linear );

        /// <summary>
        /// Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with a
        /// nearest filter. 
        /// </summary>
        public readonly static TextureFilter MipMapNearestNearest = new TextureFilter( "MipMapNearestNearest", InnerEnum.MipMapNearestNearest, IGL20.GL_Nearest_Mipmap_Nearest );

        /// <summary>
        /// Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with a
        /// linear filter. 
        /// </summary>
        public readonly static TextureFilter MipMapLinearNearest = new TextureFilter( "MipMapLinearNearest", InnerEnum.MipMapLinearNearest, IGL20.GL_Linear_Mipmap_Nearest );

        /// <summary>
        /// Fetch the two best fitting images from the mip map chain and then sample the nearest texel from each of the two images,
        /// combining them to the final output pixel. 
        /// </summary>
        public readonly static TextureFilter MipMapNearestLinear = new TextureFilter( "MipMapNearestLinear", InnerEnum.MipMapNearestLinear, IGL20.GL_Nearest_Mipmap_Linear );

        /// <summary>
        /// Fetch the two best fitting images from the mip map chain and then sample the four nearest texels from each of the two
        /// images, combining them to the final output pixel. 
        /// </summary>
        public readonly static TextureFilter MipMapLinearLinear = new TextureFilter( "MipMapLinearLinear", InnerEnum.MipMapLinearLinear, IGL20.GL_Linear_Mipmap_Linear );

        private readonly static List< TextureFilter > valueList = new List< TextureFilter >();

        static TextureFilter()
        {
            valueList.Add( Nearest );
            valueList.Add( Linear );
            valueList.Add( MipMap );
            valueList.Add( MipMapNearestNearest );
            valueList.Add( MipMapLinearNearest );
            valueList.Add( MipMapNearestLinear );
            valueList.Add( MipMapLinearLinear );
        }

        public enum InnerEnum
        {
            Nearest,
            Linear,
            MipMap,
            MipMapNearestNearest,
            MipMapLinearNearest,
            MipMapNearestLinear,
            MipMapLinearLinear
        }

        public readonly  InnerEnum innerEnumValue;
        private readonly string    _nameValue;
        private readonly int       _ordinalValue;
        private static   int       _nextOrdinal = 0;

        internal readonly int glEnum;

        internal TextureFilter( string name, InnerEnum innerEnum, int glEnum )
        {
            this.glEnum = glEnum;

            _nameValue     = name;
            _ordinalValue  = _nextOrdinal++;
            innerEnumValue = innerEnum;
        }

        public bool IsMipMap()
        {
            return glEnum != IGL20.GL_Nearest && glEnum != IGL20.GL_Linear;
        }

        public int GLEnum => glEnum;

        public static TextureFilter[] Values()
        {
            return valueList.ToArray();
        }

        public int Ordinal()
        {
            return _ordinalValue;
        }

        public override string ToString()
        {
            return _nameValue;
        }

        public static TextureFilter ValueOf( string name )
        {
            foreach ( var enumInstance in TextureFilter.valueList )
            {
                if ( enumInstance._nameValue == name )
                {
                    return enumInstance;
                }
            }

            throw new System.ArgumentException( name );
        }
    }
}
