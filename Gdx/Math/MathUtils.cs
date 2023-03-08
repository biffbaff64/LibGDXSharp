namespace LibGDXSharp.Math
{
    public class MathUtils
    {
        public const float NanoToSec            = 1 / 1000000000f;
        public const float Float_Rounding_Error = 0.000001f; // 32 bits
        public const float Pi                   = 3.1415927f;
        public const float Pi2                  = Pi * 2;

        public const float E = 2.7182818f;

        private const int Sin_Bits  = 14; // 16KB. Adjust for accuracy.
        private const int Sin_Mask  = ~( -1 << Sin_Bits );
        private const int Sin_Count = Sin_Mask + 1;

        private const float RadFull    = Pi * 2;
        private const float DegFull    = 360;
        private const float RadToIndex = Sin_Count / RadFull;
        private const float DegToIndex = Sin_Count / DegFull;

        // multiply by this to convert from radians to degrees.
        public const float RadiansToDegrees = 180f / Pi;
        public const float RadDeg           = RadiansToDegrees;

        // multiply by this to convert from degrees to radians.
        public const float DegreesToRadians = Pi / 180;
        public const float DegRad           = DegreesToRadians;

        /// <summary>
        /// Returns the sine in radians from a lookup table. 
        /// </summary>
        public static float Sin( float radians )
        {
            return SinClass.Table[ ( int )( radians * RadToIndex ) & Sin_Mask ];
        }

        /// <summary>
        /// Returns the cosine in radians from a lookup table. 
        /// </summary>
        public static float Cos( float radians )
        {
            return SinClass.Table[ ( int )( ( radians + Pi / 2 ) * RadToIndex ) & Sin_Mask ];
        }

        /// <summary>
        /// Returns the sine in radians from a lookup table.
        /// </summary>
        public static float SinDeg( float degrees )
        {
            return SinClass.Table[ ( int )( degrees * DegToIndex ) & Sin_Mask ];
        }

        /** Returns the cosine in radians from a lookup table. */
        public static float CosDeg( float degrees )
        {
            return SinClass.Table[ ( int )( ( degrees + 90 ) * DegToIndex ) & Sin_Mask ];
        }

        // ---

        /** Returns atan2 in radians, faster but less accurate than Math.atan2. Average error of 0.00231 radians (0.1323 degrees),
         * largest error of 0.00488 radians (0.2796 degrees). */
        public static float Atan2( float y, float x )
        {
            if ( x == 0f )
            {
                if ( y > 0f ) return Pi / 2;
                if ( y == 0f ) return 0f;

                return -Pi / 2;
            }

            float atan, z = y / x;

            if ( System.Math.Abs( z ) < 1f )
            {
                atan = z / ( 1f + 0.28f * z * z );

                if ( x < 0f ) return atan + ( y < 0f ? -Pi : Pi );

                return atan;
            }

            atan = Pi / 2 - z / ( z * z + 0.28f );

            return y < 0f ? atan - Pi : atan;
        }

        // ---

        public static Random random = new Random();

        /** Returns a random number between 0 (inclusive) and the specified value (inclusive). */
        public static int Random( int range )
        {
            return random.Next( range + 1 );
        }

        /** Returns a random number between start (inclusive) and end (inclusive). */
        public static int Random( int start, int end )
        {
            return start + random.Next( end - start + 1 );
        }

        /** Returns a random number between 0 (inclusive) and the specified value (inclusive). */
        public static long Random( long range )
        {
            return ( long )( random.NextDouble() * range );
        }

        /** Returns a random number between start (inclusive) and end (inclusive). */
        public static long Random( long start, long end )
        {
            return start + ( long )( random.NextDouble() * ( end - start ) );
        }

        /** Returns a random bool value. */
        public static bool RandomBoolean()
        {
            return Convert.ToBoolean( random.Next( 1 ) );
        }

        /** Returns true if a random value between 0 and 1 is less than the specified value. */
        public static bool RandomBoolean( float chance )
        {
            return MathUtils.Random() < chance;
        }

        /** Returns random number between 0.0 (inclusive) and 1.0 (exclusive). */
        public static float Random()
        {
            return ( float )random.NextDouble();
        }

        /** Returns a random number between 0 (inclusive) and the specified value (exclusive). */
        public static float Random( float range )
        {
            return ( float )random.NextDouble() * range;
        }

        /** Returns a random number between start (inclusive) and end (exclusive). */
        public static float Random( float start, float end )
        {
            return start + ( float )random.NextDouble() * ( end - start );
        }

        /** Returns -1 or 1, randomly. */
        public static int RandomSign()
        {
            return 1 | ( random.Next() >> 31 );
        }

        /** Returns a triangularly distributed random number between -1.0 (exclusive) and 1.0 (exclusive), where values around zero are
         * more likely.
         * <p>
         * This is an optimized version of {@link #randomTriangular(float, float, float) randomTriangular(-1, 1, 0)} */
        public static float RandomTriangular()
        {
            return ( float )random.NextDouble() - ( float )random.NextDouble();
        }

        /** Returns a triangularly distributed random number between {@code -max} (exclusive) and {@code max} (exclusive), where values
         * around zero are more likely.
         * <p>
         * This is an optimized version of {@link #randomTriangular(float, float, float) randomTriangular(-max, max, 0)}
         * @param max the upper limit */
        public static float RandomTriangular( float max )
        {
            return ( float )( random.NextDouble() - random.NextDouble() ) * max;
        }

        /** Returns a triangularly distributed random number between {@code min} (inclusive) and {@code max} (exclusive), where the
         * {@code mode} argument defaults to the midpoint between the bounds, giving a symmetric distribution.
         * <p>
         * This method is equivalent of {@link #randomTriangular(float, float, float) randomTriangular(min, max, (min + max) * .5f)}
         * @param min the lower limit
         * @param max the upper limit */
        public static float RandomTriangular( float min, float max )
        {
            return RandomTriangular( min, max, ( min + max ) * 0.5f );
        }

        /** Returns a triangularly distributed random number between {@code min} (inclusive) and {@code max} (exclusive), where values
         * around {@code mode} are more likely.
         * @param min the lower limit
         * @param max the upper limit
         * @param mode the point around which the values are more likely */
        public static float RandomTriangular( float min, float max, float mode )
        {
            float u = ( float )random.NextDouble();
            float d = max - min;

            if ( u <= ( mode - min ) / d ) return min + ( float )System.Math.Sqrt( u * d * ( mode - min ) );

            return max - ( float )System.Math.Sqrt( ( 1 - u ) * d * ( max - mode ) );
        }

        // ---

        /** Returns the next power of two. Returns the specified value if the value is already a power of two. */
        public static int NextPowerOfTwo( int value )
        {
            if ( value == 0 ) return 1;
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;

            return value + 1;
        }

        public static bool IsPowerOfTwo( int value )
        {
            return value != 0 && ( value & value - 1 ) == 0;
        }

        // ---

        public static short Clamp( short value, short min, short max )
        {
            if ( value < min ) return min;
            if ( value > max ) return max;

            return value;
        }

        public static int Clamp( int value, int min, int max )
        {
            if ( value < min ) return min;
            if ( value > max ) return max;

            return value;
        }

        public static long Clamp( long value, long min, long max )
        {
            if ( value < min ) return min;
            if ( value > max ) return max;

            return value;
        }

        public static float Clamp( float value, float min, float max )
        {
            if ( value < min ) return min;
            if ( value > max ) return max;

            return value;
        }

        public static double Clamp( double value, double min, double max )
        {
            if ( value < min ) return min;
            if ( value > max ) return max;

            return value;
        }

        // ---

        /** Linearly interpolates between fromValue to toValue on progress position. */
        public static float Lerp( float fromValue, float toValue, float progress )
        {
            return fromValue + ( toValue - fromValue ) * progress;
        }

        /** Linearly interpolates between two angles in radians. Takes into account that angles wrap at two pi and always takes the
         * direction with the smallest delta angle.
         * 
         * @param fromRadians start angle in radians
         * @param toRadians target angle in radians
         * @param progress interpolation value in the range [0, 1]
         * @return the interpolated angle in the range [0, PI2[ */
        public static float LerpAngle( float fromRadians, float toRadians, float progress )
        {
            float delta = ( ( toRadians - fromRadians + Pi2 + Pi ) % Pi2 ) - Pi;

            return ( fromRadians + delta * progress + Pi2 ) % Pi2;
        }

        /** Linearly interpolates between two angles in degrees. Takes into account that angles wrap at 360 degrees and always takes
         * the direction with the smallest delta angle.
         * 
         * @param fromDegrees start angle in degrees
         * @param toDegrees target angle in degrees
         * @param progress interpolation value in the range [0, 1]
         * @return the interpolated angle in the range [0, 360[ */
        public static float LerpAngleDeg( float fromDegrees, float toDegrees, float progress )
        {
            float delta = ( ( toDegrees - fromDegrees + 360 + 180 ) % 360 ) - 180;

            return ( fromDegrees + delta * progress + 360 ) % 360;
        }

        // ---

        private const int    Big_Enough_Int   = 16 * 1024;
        private const double Big_Enough_Floor = Big_Enough_Int;
        private const double Ceiling          = 0.9999999;
        private const double Big_Enough_Ceil  = 16384.999999999996;
        private const double Big_Enough_Round = Big_Enough_Int + 0.5f;

        /** Returns the largest integer less than or equal to the specified float. This method will only properly floor floats from
         * -(2^14) to (Float.MAX_VALUE - 2^14). */
        public static int Floor( float value )
        {
            return ( int )( value + Big_Enough_Floor ) - Big_Enough_Int;
        }

        /** Returns the largest integer less than or equal to the specified float. This method will only properly floor floats that are
         * positive. Note this method simply casts the float to int. */
        public static int FloorPositive( float value )
        {
            return ( int )value;
        }

        /** Returns the smallest integer greater than or equal to the specified float. This method will only properly ceil floats from
         * -(2^14) to (Float.MAX_VALUE - 2^14). */
        public static int Ceil( float value )
        {
            return ( int )( value + Big_Enough_Ceil ) - Big_Enough_Int;
        }

        /** Returns the smallest integer greater than or equal to the specified float. This method will only properly ceil floats that
         * are positive. */
        public static int CeilPositive( float value )
        {
            return ( int )( value + Ceiling );
        }

        /** Returns the closest integer to the specified float. This method will only properly round floats from -(2^14) to
         * (Float.MAX_VALUE - 2^14). */
        public static int Round( float value )
        {
            return ( int )( value + Big_Enough_Round ) - Big_Enough_Int;
        }

        /** Returns the closest integer to the specified float. This method will only properly round floats that are positive. */
        public static int RoundPositive( float value )
        {
            return ( int )( value + 0.5f );
        }

        /** Returns true if the value is zero (using the default tolerance as upper bound) */
        public static bool IsZero( float value )
        {
            return System.Math.Abs( value ) <= Float_Rounding_Error;
        }

        /** Returns true if the value is zero.
         * @param tolerance represent an upper bound below which the value is considered zero. */
        public static bool IsZero( float value, float tolerance )
        {
            return System.Math.Abs( value ) <= tolerance;
        }

        /** Returns true if a is nearly equal to b. The function uses the default floating error tolerance.
         * @param a the first value.
         * @param b the second value. */
        public static bool IsEqual( float a, float b )
        {
            return System.Math.Abs( a - b ) <= Float_Rounding_Error;
        }

        /** Returns true if a is nearly equal to b.
         * @param a the first value.
         * @param b the second value.
         * @param tolerance represent an upper bound below which the two values are considered equal. */
        public static bool IsEqual( float a, float b, float tolerance )
        {
            return System.Math.Abs( a - b ) <= tolerance;
        }

        /** @return the logarithm of value with base a */
        public static float Log( float a, float value )
        {
            return ( float )( System.Math.Log( value ) / System.Math.Log( a ) );
        }

        /** @return the logarithm of value with base 2 */
        public static float Log2( float value )
        {
            return Log( 2, value );
        }

        internal class SinClass
        {
            public readonly static float[] Table = new float[ Sin_Count ];

            public SinClass()
            {
                for ( int i = 0; i < Sin_Count; i++ )
                {
                    Table[ i ] = ( float )System.Math.Sin( ( i + 0.5f ) / Sin_Count * RadFull );
                }

                for ( int i = 0; i < 360; i += 90 )
                {
                    Table[ ( int )( i * DegToIndex ) & Sin_Mask ] = ( float )System.Math.Sin( i * DegreesToRadians );
                }
            }
        }
    }
}
