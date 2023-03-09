namespace LibGDXSharp.Maths
{
    public class Vector3 : IVector< Vector3 >
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public readonly static Vector3 XDefault = new Vector3( 1, 0, 0 );
        public readonly static Vector3 YDefault = new Vector3( 0, 1, 0 );
        public readonly static Vector3 ZDefault = new Vector3( 0, 0, 1 );
        public readonly static Vector3 Zero     = new Vector3( 0, 0, 0 );

        private readonly static Matrix4 _tmpMat = new Matrix4();

        public Vector3()
        {
        }

        public Vector3( float x, float y, float z )
        {
            this.Set( x, y, z );
        }

        public Vector3( Vector3 vector )
        {
            this.Set( vector );
        }

        public Vector3( float[] values )
        {
            this.Set( values[ 0 ], values[ 1 ], values[ 2 ] );
        }

        public Vector3( Vector2 vector, float z )
        {
            this.Set( vector.X, vector.Y, z );
        }

        public Vector3 Set( float x, float y, float z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            return this;
        }

        public Vector3 Set( Vector3 vector )
        {
            return this.Set( vector.X, vector.Y, vector.Z );
        }

        public Vector3 Set( float[] values )
        {
            return this.Set( values[ 0 ], values[ 1 ], values[ 2 ] );
        }

        public Vector3 Set( Vector2 vector, float z )
        {
            return this.Set( vector.X, vector.Y, z );
        }

        public Vector3 SetFromSpherical( float azimuthalAngle, float polarAngle )
        {
            var cosPolar = MathUtils.Cos( polarAngle );
            var sinPolar = MathUtils.Sin( polarAngle );

            var cosAzim = MathUtils.Cos( azimuthalAngle );
            var sinAzim = MathUtils.Sin( azimuthalAngle );

            return this.Set( cosAzim * sinPolar, sinAzim * sinPolar, cosPolar );
        }

        public Vector3 SetToRandomDirection()
        {
            var u = MathUtils.Random();
            var v = MathUtils.Random();

            var theta = MathUtils.PI2 * u;                 // azimuthal angle
            var phi   = ( float )Math.Acos( 2f * v - 1f ); // polar angle

            return this.SetFromSpherical( theta, phi );
        }

        public Vector3 Cpy()
        {
            return new Vector3( this );
        }

        public Vector3 Add( Vector3 vector )
        {
            return this.Add( vector.X, vector.Y, vector.Z );
        }

        public Vector3 Add( float x, float y, float z )
        {
            return this.Set( this.X + x, this.Y + y, this.Z + z );
        }

        public Vector3 Add( float values )
        {
            return this.Set( this.X + values, this.Y + values, this.Z + values );
        }

        public Vector3 Sub( Vector3 vec )
        {
            return this.Sub( vec.X, vec.Y, vec.Z );
        }

        public Vector3 Sub( float x, float y, float z )
        {
            return this.Set( this.X - x, this.Y - y, this.Z - z );
        }

        public Vector3 Sub( float value )
        {
            return this.Set( this.X - value, this.Y - value, this.Z - value );
        }

        public Vector3 Scl( float scalar )
        {
            return this.Set( this.X * scalar, this.Y * scalar, this.Z * scalar );
        }

        public Vector3 Scl( Vector3 other )
        {
            return this.Set( X * other.X, Y * other.Y, Z * other.Z );
        }

        public Vector3 Scl( float vx, float vy, float vz )
        {
            return this.Set( this.X * vx, this.Y * vy, this.Z * vz );
        }

        public Vector3 MulAdd( Vector3 vec, float scalar )
        {
            this.X += vec.X * scalar;
            this.Y += vec.Y * scalar;
            this.Z += vec.Z * scalar;

            return this;
        }

        public Vector3 MulAdd( Vector3 vec, Vector3 mulVec )
        {
            this.X += vec.X * mulVec.X;
            this.Y += vec.Y * mulVec.Y;
            this.Z += vec.Z * mulVec.Z;

            return this;
        }

        public static float Len( float x, float y, float z )
        {
            return ( float )Math.Sqrt( x * x + y * y + z * z );
        }

        public float Len()
        {
            return ( float )Math.Sqrt( X * X + Y * Y + Z * Z );
        }

        public static float Len2( float x, float y, float z )
        {
            return x * x + y * y + z * z;
        }

        public float Len2()
        {
            return X * X + Y * Y + Z * Z;
        }

        public bool Idt( Vector3 vector )
        {
            return X == vector.X && Y == vector.Y && Z == vector.Z;
        }

        public static float Dst( float x1, float y1, float z1, float x2, float y2, float z2 )
        {
            var a = x2 - x1;
            var b = y2 - y1;
            var c = z2 - z1;

            return ( float )Math.Sqrt( a * a + b * b + c * c );
        }

        public float Dst( Vector3 vector )
        {
            var a = vector.X - X;
            var b = vector.Y - Y;
            var c = vector.Z - Z;

            return ( float )Math.Sqrt( a * a + b * b + c * c );
        }

        public float Dst( float x, float y, float z )
        {
            var a = x - this.X;
            var b = y - this.Y;
            var c = z - this.Z;

            return ( float )Math.Sqrt( a * a + b * b + c * c );
        }

        public static float Dst2( float x1, float y1, float z1, float x2, float y2, float z2 )
        {
            var a = x2 - x1;
            var b = y2 - y1;
            var c = z2 - z1;

            return a * a + b * b + c * c;
        }

        public float Dst2( Vector3 point )
        {
            var a = point.X - X;
            var b = point.Y - Y;
            var c = point.Z - Z;

            return a * a + b * b + c * c;
        }

        public float Dst2( float x, float y, float z )
        {
            var a = x - this.X;
            var b = y - this.Y;
            var c = z - this.Z;

            return a * a + b * b + c * c;
        }

        public Vector3 Nor()
        {
            var len2 = this.Len2();

            if ( len2 == 0f || len2 == 1f ) return this;

            return this.Scl( 1f / ( float )Math.Sqrt( len2 ) );
        }

        public static float Dot( float x1, float y1, float z1, float x2, float y2, float z2 )
        {
            return x1 * x2 + y1 * y2 + z1 * z2;
        }

        public float Dot( Vector3 vector )
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        public float Dot( float x, float y, float z )
        {
            return this.X * x + this.Y * y + this.Z * z;
        }

        public Vector3 Crs( Vector3 vector )
        {
            return this.Set( Y * vector.Z - Z * vector.Y, Z * vector.X - X * vector.Z, X * vector.Y - Y * vector.X );
        }

        public Vector3 Crs( float x, float y, float z )
        {
            return this.Set( this.Y * z - this.Z * y, this.Z * x - this.X * z, this.X * y - this.Y * x );
        }

        public Vector3 Mul4X3( float[] matrix )
        {
            return Set
                (
                 X * matrix[ 0 ] + Y * matrix[ 3 ] + Z * matrix[ 6 ] + matrix[ 9 ],
                 X * matrix[ 1 ]
                 + Y * matrix[ 4 ]
                 + Z * matrix[ 7 ]
                 + matrix[ 10 ],
                 X * matrix[ 2 ] + Y * matrix[ 5 ] + Z * matrix[ 8 ] + matrix[ 11 ]
                );
        }

        public Vector3 Mul( Matrix4 matrix )
        {
            var lMat = matrix.val;

            return this.Set
                (
                 X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M01 ] + Z * lMat[ Matrix4.M02 ] + lMat[ Matrix4.M03 ],
                 X
                 * lMat[ Matrix4.M10 ]
                 + Y * lMat[ Matrix4.M11 ]
                 + Z * lMat[ Matrix4.M12 ]
                 + lMat[ Matrix4.M13 ],
                 X * lMat[ Matrix4.M20 ]
                 + Y
                 * lMat[ Matrix4.M21 ]
                 + Z * lMat[ Matrix4.M22 ]
                 + lMat[ Matrix4.M23 ]
                );
        }

        public Vector3 TraMul( Matrix4 matrix )
        {
            var lMat = matrix.val;

            return this.Set
                (
                 X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M10 ] + Z * lMat[ Matrix4.M20 ] + lMat[ Matrix4.M30 ],
                 X
                 * lMat[ Matrix4.M01 ]
                 + Y * lMat[ Matrix4.M11 ]
                 + Z * lMat[ Matrix4.M21 ]
                 + lMat[ Matrix4.M31 ],
                 X * lMat[ Matrix4.M02 ]
                 + Y
                 * lMat[ Matrix4.M12 ]
                 + Z * lMat[ Matrix4.M22 ]
                 + lMat[ Matrix4.M32 ]
                );
        }

        public Vector3 Mul( Matrix3 matrix )
        {
            var lMat = matrix.val;

            return Set
                (
                 X * lMat[ Matrix3.M00 ] + Y * lMat[ Matrix3.M01 ] + Z * lMat[ Matrix3.M02 ],
                 X * lMat[ Matrix3.M10 ]
                 + Y
                 * lMat[ Matrix3.M11 ]
                 + Z * lMat[ Matrix3.M12 ],
                 X * lMat[ Matrix3.M20 ] + Y * lMat[ Matrix3.M21 ] + Z * lMat[ Matrix3.M22 ]
                );
        }

        public Vector3 TraMul( Matrix3 matrix )
        {
            var lMat = matrix.val;

            return Set
                (
                 X * lMat[ Matrix3.M00 ] + Y * lMat[ Matrix3.M10 ] + Z * lMat[ Matrix3.M20 ],
                 X * lMat[ Matrix3.M01 ]
                 + Y
                 * lMat[ Matrix3.M11 ]
                 + Z * lMat[ Matrix3.M21 ],
                 X * lMat[ Matrix3.M02 ] + Y * lMat[ Matrix3.M12 ] + Z * lMat[ Matrix3.M22 ]
                );
        }

        public Vector3 Mul( Quaternion quat )
        {
            return quat.Transform( this );
        }

        public Vector3 Prj( Matrix4 matrix )
        {
            var lMat = matrix.val;
            var lW = 1f / ( X * lMat[ Matrix4.M30 ] + Y * lMat[ Matrix4.M31 ] + Z * lMat[ Matrix4.M32 ] + lMat[ Matrix4.M33 ] );

            return this.Set
                (
                 ( X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M01 ] + Z * lMat[ Matrix4.M02 ] + lMat[ Matrix4.M03 ] ) * lW,
                 ( X
                   * lMat[ Matrix4.M10 ]
                   + Y * lMat[ Matrix4.M11 ]
                   + Z * lMat[ Matrix4.M12 ]
                   + lMat[ Matrix4.M13 ] )
                 * lW,
                 ( X * lMat[ Matrix4.M20 ] + Y * lMat[ Matrix4.M21 ] + Z * lMat[ Matrix4.M22 ] + lMat[ Matrix4.M23 ] ) * lW
                );
        }

        public Vector3 Rot( Matrix4 matrix )
        {
            var lMat = matrix.val;

            return this.Set
                (
                 X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M01 ] + Z * lMat[ Matrix4.M02 ],
                 X * lMat[ Matrix4.M10 ]
                 + Y
                 * lMat[ Matrix4.M11 ]
                 + Z * lMat[ Matrix4.M12 ],
                 X * lMat[ Matrix4.M20 ] + Y * lMat[ Matrix4.M21 ] + Z * lMat[ Matrix4.M22 ]
                );
        }

        public Vector3 Unrotate( Matrix4 matrix )
        {
            var lMat = matrix.val;

            return this.Set
                (
                 X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M10 ] + Z * lMat[ Matrix4.M20 ],
                 X * lMat[ Matrix4.M01 ]
                 + Y
                 * lMat[ Matrix4.M11 ]
                 + Z * lMat[ Matrix4.M21 ],
                 X * lMat[ Matrix4.M02 ] + Y * lMat[ Matrix4.M12 ] + Z * lMat[ Matrix4.M22 ]
                );
        }

        public Vector3 Untransform( Matrix4 matrix )
        {
            var lMat = matrix.val;

            X -= lMat[ Matrix4.M03 ];
            Y -= lMat[ Matrix4.M03 ];
            Z -= lMat[ Matrix4.M03 ];

            return this.Set
                (
                 X * lMat[ Matrix4.M00 ] + Y * lMat[ Matrix4.M10 ] + Z * lMat[ Matrix4.M20 ],
                 X * lMat[ Matrix4.M01 ]
                 + Y
                 * lMat[ Matrix4.M11 ]
                 + Z * lMat[ Matrix4.M21 ],
                 X * lMat[ Matrix4.M02 ] + Y * lMat[ Matrix4.M12 ] + Z * lMat[ Matrix4.M22 ]
                );
        }

        public Vector3 Rotate( float degrees, float axisX, float axisY, float axisZ )
        {
            return this.Mul( _tmpMat.SetToRotation( axisX, axisY, axisZ, degrees ) );
        }

        public Vector3 RotateRad( float radians, float axisX, float axisY, float axisZ )
        {
            return this.Mul( _tmpMat.SetToRotationRad( axisX, axisY, axisZ, radians ) );
        }

        public Vector3 Rotate( Vector3 axis, float degrees )
        {
            _tmpMat.SetToRotation( axis, degrees );

            return this.Mul( _tmpMat );
        }

        public Vector3 RotateRad( Vector3 axis, float radians )
        {
            _tmpMat.SetToRotationRad( axis, radians );

            return this.Mul( _tmpMat );
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
            return X == 0 && Y == 0 && Z == 0;
        }

        public bool IsZero( float margin )
        {
            return Len2() < margin;
        }

        public bool IsOnLine( Vector3 other, float epsilon )
        {
            return Len2( Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X ) <= epsilon;
        }

        public bool IsOnLine( Vector3 other )
        {
            return Len2( Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X ) <= MathUtils.Float_Rounding_Error;
        }

        public bool IsCollinear( Vector3 other, float epsilon )
        {
            return IsOnLine( other, epsilon ) && HasSameDirection( other );
        }

        public bool IsCollinear( Vector3 other )
        {
            return IsOnLine( other ) && HasSameDirection( other );
        }

        public bool IsCollinearOpposite( Vector3 other, float epsilon )
        {
            return IsOnLine( other, epsilon ) && HasOppositeDirection( other );
        }

        public bool IsCollinearOpposite( Vector3 other )
        {
            return IsOnLine( other ) && HasOppositeDirection( other );
        }

        public bool IsPerpendicular( Vector3 vector )
        {
            return MathUtils.IsZero( Dot( vector ) );
        }

        public bool IsPerpendicular( Vector3 vector, float epsilon )
        {
            return MathUtils.IsZero( Dot( vector ), epsilon );
        }

        public bool HasSameDirection( Vector3 vector )
        {
            return Dot( vector ) > 0;
        }

        public bool HasOppositeDirection( Vector3 vector )
        {
            return Dot( vector ) < 0;
        }

        public Vector3 Lerp( Vector3 target, float alpha )
        {
            X += alpha * ( target.X - X );
            Y += alpha * ( target.Y - Y );
            Z += alpha * ( target.Z - Z );

            return this;
        }

        public Vector3 Interpolate( Vector3 target, float alpha, Interpolation interpolator )
        {
            return Lerp( target, interpolator.Apply( 0f, 1f, alpha ) );
        }

        public Vector3 Slerp( Vector3 target, float alpha )
        {
            float dot = Dot( target );

            // If the inputs are too close for comfort, simply linearly interpolate.
            if ( dot > 0.9995 || dot < -0.9995 ) return Lerp( target, alpha );

            // theta0 = angle between input vectors
            var theta0 = ( float )Math.Acos( dot );
            // theta = angle between this vector and result
            var theta = theta0 * alpha;

            var st = ( float )Math.Sin( theta );
            var tx = target.X - X * dot;
            var ty = target.Y - Y * dot;
            var tz = target.Z - Z * dot;
            var l2 = tx * tx + ty * ty + tz * tz;
            var dl = st * ( ( l2 < 0.0001f ) ? 1f : 1f / ( float )Math.Sqrt( l2 ) );

            return Scl( ( float )Math.Cos( theta ) ).Add( tx * dl, ty * dl, tz * dl ).Nor();
        }

        public new string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }

        public Vector3 FromString( string v )
        {
            int s0 = v.IndexOf( ',', 1 );
            int s1 = v.IndexOf( ',', s0 + 1 );

            if ( s0 != -1 && s1 != -1 && v[ 0 ] == '(' && v[ ^1 ] == ')' )
            {
                try
                {
                    float x = float.Parse( v.Substring( 1, s0 ) );
                    float y = float.Parse( v.Substring( s0 + 1, s1 ) );
                    float z = float.Parse( v.Substring( s1 + 1, v.Length - 1 ) );

                    return this.Set( x, y, z );
                }
                catch ( NumberFormatException ex )
                {
                    // Throw a GdxRuntimeException
                }
            }

            throw new GdxRuntimeException( "Malformed Vector3: " + v );
        }

        public Vector3 Limit( float limit )
        {
            return Limit2( limit * limit );
        }

        public Vector3 Limit2( float limit2 )
        {
            float len2 = Len2();

            if ( len2 > limit2 )
            {
                Scl( ( float )Math.Sqrt( limit2 / len2 ) );
            }

            return this;
        }

        public Vector3 SetLength( float len )
        {
            return SetLength2( len * len );
        }

        public Vector3 SetLength2( float len2 )
        {
            float oldLen2 = Len2();

            return ( oldLen2 == 0 || oldLen2 == len2 ) ? this : Scl( ( float )Math.Sqrt( len2 / oldLen2 ) );
        }

        public Vector3 Clamp( float min, float max )
        {
            float len2 = Len2();

            if ( len2 == 0f ) return this;

            var max2 = max * max;

            if ( len2 > max2 ) return Scl( ( float )Math.Sqrt( max2 / len2 ) );

            var min2 = min * min;

            if ( len2 < min2 ) return Scl( ( float )Math.Sqrt( min2 / len2 ) );

            return this;
        }

        public int HashCode()
        {
            const int prime  = 31;
            var       result = 1;

            result = prime + NumberUtils.FloatToIntBits( X );
            result = prime * result + NumberUtils.FloatToIntBits( Y );
            result = prime * result + NumberUtils.FloatToIntBits( Z );

            return result;
        }

        public new bool Equals( object? obj )
        {
            if ( this == obj ) return true;
            if ( obj == null ) return false;
            if ( GetType() != obj.GetType() ) return false;

            var other = ( Vector3 )obj;

            if ( NumberUtils.FloatToIntBits( X ) != NumberUtils.FloatToIntBits( other.X ) ) return false;
            if ( NumberUtils.FloatToIntBits( Y ) != NumberUtils.FloatToIntBits( other.Y ) ) return false;
            if ( NumberUtils.FloatToIntBits( Z ) != NumberUtils.FloatToIntBits( other.Z ) ) return false;

            return true;
        }

        public bool EpsilonEquals( Vector3? other, float epsilon )
        {
            if ( other == null ) return false;
            if ( Math.Abs( other.X - X ) > epsilon ) return false;
            if ( Math.Abs( other.Y - Y ) > epsilon ) return false;
            if ( Math.Abs( other.Z - Z ) > epsilon ) return false;

            return true;
        }

        public bool EpsilonEquals( float x, float y, float z, float epsilon )
        {
            if ( Math.Abs( x - this.X ) > epsilon ) return false;
            if ( Math.Abs( y - this.Y ) > epsilon ) return false;
            if ( Math.Abs( z - this.Z ) > epsilon ) return false;

            return true;
        }

        public bool EpsilonEquals( Vector3 other )
        {
            return EpsilonEquals( other, MathUtils.Float_Rounding_Error );
        }

        public bool EpsilonEquals( float x, float y, float z )
        {
            return EpsilonEquals( x, y, z, MathUtils.Float_Rounding_Error );
        }

        public Vector3 SetZero()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;

            return this;
        }
    }
}
