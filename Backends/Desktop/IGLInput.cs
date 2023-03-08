namespace LibGDXSharp.Backends.Desktop
{
    public interface IGLInput : IInput, IDisposable
    {
        void WindowHandleChanged (long windowHandle);

        void Update ();

        void PrepareNext ();

        void ResetPollingStates ();
    }
}

