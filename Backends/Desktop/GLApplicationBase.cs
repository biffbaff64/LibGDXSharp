using LibGDXSharp.Backends.Desktop.Audio;

namespace LibGDXSharp.Backends.Desktop
{
    public abstract class GLApplicationBase : Application
    {
        public abstract IGLAudio CreateAudio( GLApplicationConfiguration config );

        public abstract IGLInput CreateInput( GLWindow window );
    }
}
