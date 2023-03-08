namespace LibGDXSharp
{
    /// <summary>
    /// Environment class holding references to the Application,
    /// Graphics, Audio, Files and Input instances.
    /// </summary>
    public static class Gdx
    {
        public static Application? App      { get; set; }
        public static IGraphics?   Graphics { get; set; }
        public static IAudio?      Audio    { get; set; }
        public static IInput?      Input    { get; set; }
        public static IFile?       Files    { get; set; }
        public static IGL20?       Gl       { get; set; }
        public static IGL20?       Gl20     { get; set; }
        public static IGL30?       Gl30     { get; set; }
    }
}
