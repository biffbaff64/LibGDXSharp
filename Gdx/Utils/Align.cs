namespace LibGDXSharp.Utils
{
    public class Align
    {
        public const int Center = 1 << 0;
        public const int Top    = 1 << 1;
        public const int Bottom = 1 << 2;
        public const int Left   = 1 << 3;
        public const int Right  = 1 << 4;

        public const int TopLeft     = Top | Left;
        public const int TopRight    = Top | Right;
        public const int BottomLeft  = Bottom | Left;
        public const int BottomRight = Bottom | Right;
    }
}

