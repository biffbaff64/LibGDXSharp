namespace LibGDXSharp.Maths
{
    public class Matrix4
    {
        /// <summary>
        /// XX: Typically the unrotated X component for scaling, also the cosine
        /// of the angle when rotated on the Y and/or Z axis. On Vector3 multiplication
        /// this value is multiplied with the source X component and added to the
        /// target X component. 
        /// </summary>
        public const int M00 = 0;
        /// <summary>
        /// XY: Typically the negative sine of the angle when rotated on the Z axis.
        /// On Vector3 multiplication this value is multiplied with the source Y
        /// component and added to the target X component. 
        /// </summary>
        public const int M01 = 4;
        /// <summary>
        /// XZ: Typically the sine of the angle when rotated on the Y axis.
        /// On Vector3 multiplication this value is multiplied with the
        /// source Z component and added to the target X component. 
        /// </summary>
        public const int M02 = 8;
        /// <summary>
        /// XW: Typically the translation of the X component. On Vector3 multiplication
        /// this value is added to the target X component.
        /// </summary>
        public const int M03 = 12;
        /// <summary>
        /// YX: Typically the sine of the angle when rotated on the Z axis. On Vector3
        /// multiplication this value is multiplied with the source X component and
        /// added to the target Y component. 
        /// </summary>
        public const int M10 = 1;
        /// <summary>
        /// YY: Typically the unrotated Y component for scaling, also the cosine of the
        /// angle when rotated on the X and/or Z axis. On Vector3 multiplication this value
        /// is multiplied with the source Y component and added to the target Y component. 
        /// </summary>
        public const int M11 = 5;
        /// <summary>
        /// YZ: Typically the negative sine of the angle when rotated on the X axis.
        /// On Vector3 multiplication this value is multiplied with the source Z component
        /// and added to the target Y component. 
        /// </summary>
        public const int M12 = 9;
        /// <summary>
        /// YW: Typically the translation of the Y component.
        /// On Vector3 multiplication this value is added to the target Y component.
        /// </summary>
        public const int M13 = 13;
        /// <summary>
        /// ZX: Typically the negative sine of the angle when rotated on the Y axis.
        /// On Vector3 multiplication this value is multiplied with the source X component
        /// and added to the target Z component. 
        /// </summary>
        public const int M20 = 2;
        /// <summary>
        /// ZY: Typical the sine of the angle when rotated on the X axis.
        /// On Vector3 multiplication this value is multiplied with the source Y component
        /// and added to the target Z component. 
        /// </summary>
        public const int M21 = 6;
        /// <summary>
        /// ZZ: Typically the unrotated Z component for scaling, also the cosine of the angle
        /// when rotated on the X and/or Y axis. On Vector3 multiplication this value is
        /// multiplied with the source Z component and added to the target Z component. 
        /// </summary>
        public const int M22 = 10;
        /// <summary>
        /// ZW: Typically the translation of the Z component. On Vector3 multiplication
        /// this value is added to the target Z component.
        /// </summary>
        public const int M23 = 14;
        /// <summary>
        /// WX: Typically the value zero. On Vector3 multiplication this value is ignored.
        /// </summary>
        public const int M30 = 3;
        /// <summary>
        /// WY: Typically the value zero. On Vector3 multiplication this value is ignored.
        /// </summary>
        public const int M31 = 7;
        /// <summary>
        /// WZ: Typically the value zero. On Vector3 multiplication this value is ignored.
        /// </summary>
        public const int M32 = 11;
        /// <summary>
        /// WW: Typically the value one. On Vector3 multiplication this value is ignored.
        /// </summary>
        public const int M33 = 15;

        internal readonly static Quaternion Quat       = new Quaternion();
        internal readonly static Quaternion Quat2      = new Quaternion();
        internal readonly static Vector3    LVez       = new Vector3();
        internal readonly static Vector3    LVex       = new Vector3();
        internal readonly static Vector3    LVey       = new Vector3();
        internal readonly static Vector3    TmpVec     = new Vector3();
        internal readonly static Matrix4    TmpMat     = new Matrix4();
        internal readonly static Vector3    Right      = new Vector3();
        internal readonly static Vector3    TmpForward = new Vector3();
        internal readonly static Vector3    TmpUp      = new Vector3();

        public float[] val = new float[ 16 ];

        public Matrix4 SetToRotation( Vector3 axis, float degrees )
        {
            return null;
        }

        public Matrix4 SetToRotation( float axisX, float axisY, float axisZ, float degrees )
        {
            return null;
        }

        public Matrix4 SetToRotationRad( Vector3 axis, float radians )
        {
            return null;
        }

        public Matrix4 SetToRotationRad( float axisX, float axisY, float axisZ, float radians )
        {
            return null;
        }
    }
}
