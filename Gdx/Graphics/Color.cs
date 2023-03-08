namespace LibGDXSharp.Graphics
{
    public class Color
    {
        public readonly static Color White      = new Color( 1, 1, 1, 1 );
        public readonly static Color LightGray = new Color( 0xbfbfbfff );
        public readonly static Color Gray       = new Color( 0x7f7f7fff );
        public readonly static Color DarkGray  = new Color( 0x3f3f3fff );
        public readonly static Color Black      = new Color( 0, 0, 0,    1 );
        public readonly static Color Clear      = new Color( 0, 0, 0,    0 );
        public readonly static Color Blue       = new Color( 0, 0, 1,    1 );
        public readonly static Color Navy       = new Color( 0, 0, 0.5f, 1 );
        public readonly static Color Royal      = new Color( 0x4169e1ff );
        public readonly static Color Slate      = new Color( 0x708090ff );
        public readonly static Color Sky        = new Color( 0x87ceebff );
        public readonly static Color Cyan       = new Color( 0, 1,    1,    1 );
        public readonly static Color Teal       = new Color( 0, 0.5f, 0.5f, 1 );
        public readonly static Color Green      = new Color( 0x00ff00ff );
        public readonly static Color Chartreuse = new Color( 0x7fff00ff );
        public readonly static Color Lime       = new Color( 0x32cd32ff );
        public readonly static Color Forest     = new Color( 0x228b22ff );
        public readonly static Color Olive      = new Color( 0x6b8e23ff );
        public readonly static Color Yellow     = new Color( 0xffff00ff );
        public readonly static Color Gold       = new Color( 0xffd700ff );
        public readonly static Color Goldenrod  = new Color( 0xdaa520ff );
        public readonly static Color Orange     = new Color( 0xffa500ff );
        public readonly static Color Brown      = new Color( 0x8b4513ff );
        public readonly static Color Tan        = new Color( 0xd2b48cff );
        public readonly static Color Firebrick  = new Color( 0xb22222ff );
        public readonly static Color Red        = new Color( 0xff0000ff );
        public readonly static Color Scarlet    = new Color( 0xff341cff );
        public readonly static Color Coral      = new Color( 0xff7f50ff );
        public readonly static Color Salmon     = new Color( 0xfa8072ff );
        public readonly static Color Pink       = new Color( 0xff69b4ff );
        public readonly static Color Magenta    = new Color( 1, 0, 1, 1 );
        public readonly static Color Purple     = new Color( 0xa020f0ff );
        public readonly static Color Violet     = new Color( 0xee82eeff );
        public readonly static Color Maroon     = new Color( 0xb03060ff );

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public Color() : this( 0, 0, 0, 0 )
        {
        }

        public Color( int rgba8888 ) : this( ( uint )rgba8888 )
        {
        }

        public Color( uint rgba8888 )
        {
            Rgba8888ToColor( this, rgba8888 );
        }

        public Color( float r, float g, float b, float a )
        {
            R = r;
            G = g;
            B = b;
            A = a;

            Clamp();
        }

        public Color( Color color )
        {
            Set( color );
        }

        public Color Set( Color color )
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;

            return this;
        }

        /// <summary>
        /// Sets the Color components using the specified integer value
        /// in the format RGBA8888. This is inverse to the
        /// RGBA8888(r, g, b, a) method.
        /// </summary>
        /// <param name="color">The Color to be modified.</param>
        /// <param name="value">An integer color value in RGBA8888 format.</param>
        private void Rgba8888ToColor( Color color, uint value )
        {
            color.R = ( ( value & 0xff000000 ) >> 24 ) / 255f;
            color.G = ( ( value & 0x00ff0000 ) >> 16 ) / 255f;
            color.B = ( ( value & 0x0000ff00 ) >> 8 )  / 255f;
            color.A = ( ( value & 0x000000ff ) )       / 255f;
        }

        /// <summary>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public int Rgba8888( float r, float g, float b, float a ) =>
                ( ( int )( r * 255 ) << 24 )
                | ( ( int )( g * 255 ) << 16 )
                | ( ( int )( b * 255 ) << 8 )
                | ( int )( a * 255 );

        /// <summary>
        /// Clamps this Color's components to a valid range [0 - 1]
        /// </summary>
        /// <returns>This Color for chaining.</returns>
        private Color Clamp()
        {
            if ( R < 0 )
            {
                R = 0;
            }
            else if ( R > 1 )
            {
                R = 1;
            }

            if ( G < 0 )
            {
                G = 0;
            }
            else if ( G > 1 )
            {
                G = 1;
            }

            if ( B < 0 )
            {
                B = 0;
            }
            else if ( B > 1 )
            {
                B = 1;
            }

            if ( A < 0 )
            {
                A = 0;
            }
            else if ( A > 1 )
            {
                A = 1;
            }

            return this;
        }
    }
}


