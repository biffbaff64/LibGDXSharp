namespace LibGDXSharp.Maths
{
    public class Vector2 : IVector< Vector2 >
    {
        public readonly static Vector2 XDefault = new Vector2( 1, 0 );
        public readonly static Vector2 YDefault = new Vector2( 0, 1 );
        public readonly static Vector2 Zero     = new Vector2( 0, 0 );

        public float X { get; set; } // the x-component of this vector.
        public float Y { get; set; } // the y-component of this vector.

        /// <summary>
        /// Constructs a new vector at (0,0)
        /// </summary>
        public Vector2()
        {
        }

        /// <summary>
        /// Constructs a vector with the given components
        /// </summary>
        /// <param name="x">The x-component.</param>
        /// <param name="y">The x-component.</param>
        public Vector2( float x, float y )
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a vector from the given vector
        /// </summary>
        /// <param name="v">The Vector</param>
        public Vector2( Vector2 v )
        {
            Set( v );
        }

        public Vector2 Cpy()
        {
            return new Vector2( this );
        }

        public static float Len( float x, float y )
        {
            return ( float )Math.Sqrt( x * x + y * y );
        }

        public float Len()
        {
            return ( float )Math.Sqrt( X * X + Y * Y );
        }

        public static float Len2( float x, float y )
        {
            return x * x + y * y;
        }

        public float Len2()
        {
            return X * X + Y * Y;
        }

        public Vector2 Set( Vector2 v )
        {
            X = v.X;
            Y = v.Y;

            return this;
        }

        /// <summary>
        /// Sets the components of this vector
        /// </summary>
        /// <param name="x">The x-component</param>
        /// <param name="y">The y-component</param>
        /// <returns>This vector for chaining</returns>
        public Vector2 Set( float x, float y )
        {
            this.X = x;
            this.Y = y;

            return this;
        }

        public Vector2 Sub( Vector2 v )
        {
            X -= v.X;
            Y -= v.Y;

            return this;
        }

        /// <summary>
        /// Substracts the other vector from this vector.
        /// </summary>
        /// <param name="x">The x-component of the other vector</param>
        /// <param name="y">The y-component of the other vector</param>
        /// <returns>This vector for chaining</returns> 
        public Vector2 Sub( float x, float y )
        {
            this.X -= x;
            this.Y -= y;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 Nor()
        {
            var len = Len();

            if ( len != 0 )
            {
                X /= len;
                Y /= len;
            }

            return this;
        }

        public Vector2 Add( Vector2 v )
        {
            X += v.X;
            Y += v.Y;

            return this;
        }

        /// <summary>
        /// Adds the given components to this vector
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <returns>This vector for chaining.</returns>
        public Vector2 Add( float x, float y )
        {
            this.X += x;
            this.Y += y;

            return this;
        }

        public static float Dot( float x1, float y1, float x2, float y2 )
        {
            return x1 * x2 + y1 * y2;
        }

        public float Dot( Vector2 v )
        {
            return X * v.X + Y * v.Y;
        }

        public float Dot( float ox, float oy )
        {
            return X * ox + Y * oy;
        }

        public Vector2 Scl( float scalar )
        {
            X *= scalar;
            Y *= scalar;

            return this;
        }

        public Vector2 Scl( float x, float y )
        {
            this.X *= x;
            this.Y *= y;

            return this;
        }

        public Vector2 Scl( Vector2 v )
        {
            this.X *= v.X;
            this.Y *= v.Y;

            return this;
        }

        public Vector2 MulAdd( Vector2 vec, float scalar )
        {
            this.X += vec.X * scalar;
            this.Y += vec.Y * scalar;

            return this;
        }

        public Vector2 MulAdd( Vector2 vec, Vector2 mulVec )
        {
            this.X += vec.X * mulVec.X;
            this.Y += vec.Y * mulVec.Y;

            return this;
        }

        public static float Dst( float x1, float y1, float x2, float y2 )
        {
            var xD = x2 - x1;
            var yD = y2 - y1;

            return ( float )Math.Sqrt( xD * xD + yD * yD );
        }

        public float Dst( Vector2 v )
        {
            var xD = v.X - X;
            var yD = v.Y - Y;

            return ( float )Math.Sqrt( xD * xD + yD * yD );
        }

        public float Dst( float x, float y )
        {
            var xD = x - this.X;
            var yD = y - this.Y;

            return ( float )Math.Sqrt( xD * xD + yD * yD );
        }

        public static float Dst2( float x1, float y1, float x2, float y2 )
        {
            var xD = x2 - x1;
            var yD = y2 - y1;

            return xD * xD + yD * yD;
        }

        public float Dst2( Vector2 v )
        {
            var xD = v.X - X;
            var yD = v.Y - Y;

            return xD * xD + yD * yD;
        }

        public float Dst2( float x, float y )
        {
            var xD = x - this.X;
            var yD = y - this.Y;

            return xD * xD + yD * yD;
        }

        public Vector2 Limit( float limit )
        {
            return Limit2( limit * limit );
        }

        public Vector2 Limit2( float limit2 )
        {
            var len2 = Len2();

            if ( len2 > limit2 )
            {
                return Scl( ( float )Math.Sqrt( limit2 / len2 ) );
            }

            return this;
        }

        public Vector2 Clamp( float min, float max )
        {
            var len2 = Len2();

            if ( len2 == 0f ) return this;
            var max2 = max * max;

            if ( len2 > max2 ) return Scl( ( float )Math.Sqrt( max2 / len2 ) );
            var min2 = min * min;

            if ( len2 < min2 ) return Scl( ( float )Math.Sqrt( min2 / len2 ) );

            return this;
        }

        public Vector2 SetLength( float len )
        {
            return SetLength2( len * len );
        }

        public Vector2 SetLength2( float len2 )
        {
            var oldLen2 = Len2();

            return ( oldLen2 == 0 || oldLen2 == len2 ) ? this : Scl( ( float )Math.Sqrt( len2 / oldLen2 ) );
        }

        public string Tostring()
        {
            return "(" + X + "," + Y + ")";
        }

        public Vector2 Fromstring( string v )
        {
            var s = v.IndexOf( ',', 1 );

            // Note - v[ ^1 ] is equivalent to v[ v.Length-1 ].
            if ( s != -1 && v[0] == '(' && v[ ^1 ] == ')' )
            {
                try
                {
                    var x = float.Parse( v.Substring( 1, s ) );
                    var y = float.Parse( v.Substring( s + 1, v.Length - 1 ) );

                    return this.Set( x, y );
                }
                catch ( NumberFormatException ex )
                {
                    // Throw a GdxRuntimeException
                }
            }

            throw new GdxRuntimeException( "Malformed Vector2: " + v );
        }

        public Vector2 Mul( Matrix3 mat )
        {
            var x = this.X * mat.val[ 0 ] + this.Y * mat.val[ 3 ] + mat.val[ 6 ];
            var y = this.X * mat.val[ 1 ] + this.Y * mat.val[ 4 ] + mat.val[ 7 ];
            
            this.X = x;
            this.Y = y;

            return this;
        }

        public float Crs( Vector2 v )
        {
            return this.X * v.Y - this.Y * v.X;
        }

        public float Crs( float x, float y )
        {
            return this.X * y - this.Y * x;
        }

        public float Angle()
        {
            var angle = ( float )Math.Atan2( Y, X ) * MathUtils.RadiansToDegrees;

            if ( angle < 0 ) angle += 360;

            return angle;
        }

        public float Angle( Vector2 reference )
        {
            return ( float )Math.Atan2( Crs( reference ), Dot( reference ) ) * MathUtils.RadiansToDegrees;
        }

        public float AngleDeg()
        {
            var angle = ( float )Math.Atan2( Y, X ) * MathUtils.RadiansToDegrees;

            if ( angle < 0 ) angle += 360;

            return angle;
        }

        public float AngleDeg( Vector2 reference )
        {
            var angle = ( float )Math.Atan2( reference.Crs( this ), reference.Dot( this ) ) * MathUtils.RadiansToDegrees;

            if ( angle < 0 ) angle += 360;

            return angle;
        }

        public float AngleRad()
        {
            return ( float )Math.Atan2( Y, X );
        }

        public float AngleRad( Vector2 reference )
        {
            return ( float )Math.Atan2( reference.Crs( this ), reference.Dot( this ) );
        }

        public Vector2 SetAngle( float degrees )
        {
            return SetAngleRad( degrees * MathUtils.DegreesToRadians );
        }

        public Vector2 SetAngleDeg( float degrees )
        {
            return SetAngleRad( degrees * MathUtils.DegreesToRadians );
        }

        public Vector2 SetAngleRad( float radians )
        {
            this.Set( Len(), 0f );
            this.RotateRad( radians );

            return this;
        }

        public Vector2 Rotate( float degrees )
        {
            return RotateRad( degrees * MathUtils.DegreesToRadians );
        }

        public Vector2 RotateAround( Vector2 reference, float degrees )
        {
            return this.Sub( reference ).RotateDeg( degrees ).Add( reference );
        }

        public Vector2 RotateDeg( float degrees )
        {
            return RotateRad( degrees * MathUtils.DegreesToRadians );
        }

        public Vector2 RotateRad( float radians )
        {
            var cos = ( float )Math.Cos( radians );
            var sin = ( float )Math.Sin( radians );

            var newX = this.X * cos - this.Y * sin;
            var newY = this.X * sin + this.Y * cos;

            this.X = newX;
            this.Y = newY;

            return this;
        }

        public Vector2 RotateAroundDeg( Vector2 reference, float degrees )
        {
            return this.Sub( reference ).RotateDeg( degrees ).Add( reference );
        }

        public Vector2 RotateAroundRad( Vector2 reference, float radians )
        {
            return this.Sub( reference ).RotateRad( radians ).Add( reference );
        }

        public Vector2 Rotate90( int dir )
        {
            var x = this.X;

            if ( dir >= 0 )
            {
                this.X = -Y;
                this.Y = x;
            }
            else
            {
                this.X = Y;
                this.Y = -x;
            }

            return this;
        }

        public Vector2 Lerp( Vector2 target, float alpha )
        {
            var invAlpha = 1.0f - alpha;
            this.X = ( X * invAlpha ) + ( target.X * alpha );
            this.Y = ( Y * invAlpha ) + ( target.Y * alpha );

            return this;
        }

        public Vector2 Interpolate( Vector2 target, float alpha, Interpolation interpolation )
        {
            return Lerp( target, interpolation.Apply( alpha ) );
        }

        public Vector2 SetToRandomDirection()
        {
            var theta = MathUtils.Random( 0f, MathUtils.PI2 );

            return this.Set( MathUtils.Cos( theta ), MathUtils.Sin( theta ) );
        }

        public int HashCode()
        {
            var prime  = 31;
            var result = 1;

            result = prime + NumberUtils.FloatToIntBits( X );
            result = prime * result + NumberUtils.FloatToIntBits( Y );

            return result;
        }

        public new bool Equals( object? obj )
        {
            if ( this == obj ) return true;
            if ( obj == null ) return false;
            if ( GetType() != obj.GetType() ) return false;

            var other = ( Vector2 )obj;

            if ( NumberUtils.FloatToIntBits( X ) != NumberUtils.FloatToIntBits( other.X ) ) return false;
            if ( NumberUtils.FloatToIntBits( Y ) != NumberUtils.FloatToIntBits( other.Y ) ) return false;

            return true;
        }

        public bool EpsilonEquals( Vector2? other, float epsilon = MathUtils.Float_Rounding_Error )
        {
            if ( other == null ) return false;
            if ( Math.Abs( other.X - X ) > epsilon ) return false;
            if ( Math.Abs( other.Y - Y ) > epsilon ) return false;

            return true;
        }

        public bool EpsilonEquals( float x, float y, float epsilon )
        {
            if ( Math.Abs( x - this.X ) > epsilon ) return false;
            if ( Math.Abs( y - this.Y ) > epsilon ) return false;

            return true;
        }

        public bool EpsilonEquals( float x, float y )
        {
            return EpsilonEquals( x, y, MathUtils.Float_Rounding_Error );
        }

        public bool IsUnit()
        {
            return IsUnit( 0.000000001f );
        }
        
        public bool IsUnit( float margin )
        {
            return Math.Abs( Len2() - 1f ) < margin;
        }

        public bool IsZero()
        {
            return X == 0 && Y == 0;
        }

        public bool IsZero( float margin )
        {
            return Len2() < margin;
        }

        public bool IsOnLine( Vector2 other )
        {
            return MathUtils.IsZero( X * other.Y - Y * other.X );
        }

        public bool IsOnLine( Vector2 other, float epsilon )
        {
            return MathUtils.IsZero( X * other.Y - Y * other.X, epsilon );
        }

        public bool IsCollinear( Vector2 other, float epsilon )
        {
            return IsOnLine( other, epsilon ) && Dot( other ) > 0f;
        }

        public bool IsCollinear( Vector2 other )
        {
            return IsOnLine( other ) && Dot( other ) > 0f;
        }

        public bool IsCollinearOpposite( Vector2 other, float epsilon )
        {
            return IsOnLine( other, epsilon ) && Dot( other ) < 0f;
        }

        public bool IsCollinearOpposite( Vector2 other )
        {
            return IsOnLine( other ) && Dot( other ) < 0f;
        }

        public bool IsPerpendicular( Vector2 vector )
        {
            return MathUtils.IsZero( Dot( vector ) );
        }

        public bool IsPerpendicular( Vector2 vector, float epsilon )
        {
            return MathUtils.IsZero( Dot( vector ), epsilon );
        }

        public bool HasSameDirection( Vector2 vector )
        {
            return Dot( vector ) > 0;
        }

        public bool HasOppositeDirection( Vector2 vector )
        {
            return Dot( vector ) < 0;
        }

        public Vector2 SetZero()
        {
            this.X = 0;
            this.Y = 0;

            return this;
        }
    }
}
