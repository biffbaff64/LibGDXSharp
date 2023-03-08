namespace LibGDXSharp.Utils
{
    public class NumberUtils
    {
        public static int FloatToIntBits( float value )
        {
            int result = FloatToRawIntBits( value );
            
            // Check for NaN based on values of bit fields, maximum
            // exponent and nonzero significand.
            if ( ( ( result & FloatConsts.Exp_Bit_Mask ) == FloatConsts.Exp_Bit_Mask )
                 && ( result & FloatConsts.Signif_Bit_Mask ) != 0 )
            {
                result = 0x7fc00000;
            }

            return result;
        }

        public static int FloatToRawIntBits( float value )
        {
            return BitConverter.SingleToInt32Bits( value );
        }

        public static float IntBitsToFloat( int value )
        {
            return BitConverter.Int32BitsToSingle( value );
        }
    }
}
