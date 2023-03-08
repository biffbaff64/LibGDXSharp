namespace LibGDXSharp.Utils
{
    public class FloatConsts
    {
        public const float Positive_Infinity = float.PositiveInfinity;
        public const float Negative_Infinity = float.NegativeInfinity;
        public const float NaN               = float.NaN;
        public const float Max_Value         = float.MaxValue;
        public const float Min_Value         = float.MinValue;
        public const float Min_Normal        = 1.17549435E-38f;
        public const int   Significand_Width = 24;
        public const int   Max_Exponent      = 127;
        public const int   Min_Exponent      = -126;
        public const int   Min_Sub_Exponent  = -149;
        public const int   Exp_Bias          = 127;
        public const int   Sign_Bit_Mask     = int.MinValue;
        public const int   Exp_Bit_Mask      = 2139095040;
        public const int   Signif_Bit_Mask   = 8388607;

        private FloatConsts()
        {
        }
    }
}
