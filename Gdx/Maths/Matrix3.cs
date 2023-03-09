namespace LibGDXSharp.Maths
{
    public class Matrix3
    {
        public const int M00 = 0;
        public const int M01 = 3;
        public const int M02 = 6;
        public const int M10 = 1;
        public const int M11 = 4;
        public const int M12 = 7;
        public const int M20 = 2;
        public const int M21 = 5;
        public const int M22 = 8;

        public  float[] val = new float[ 9 ];
        
        private float[] _tmp = new float[ 9 ];
    }
}
