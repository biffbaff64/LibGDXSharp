namespace LibGDXSharp
{
    public abstract class AbstractInput : IInput
    {
        protected bool[] PressedKeys     { get; set; }
        protected bool[] JustPressedKeys { get; set; }
        protected int    PressedKeyCount { get; set; }
        protected bool   KeyJustPressed  { get; set; }

        private readonly List<int> _keysToCatch;

        /// <summary>
        /// 
        /// </summary>
        protected AbstractInput()
        {
            PressedKeys     = new bool[ IInput.Keys.MaxKeycode + 1 ];
            JustPressedKeys = new bool[ IInput.Keys.MaxKeycode + 1 ];

            _keysToCatch = new List<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyPressed( int key )
        {
            if ( key == IInput.Keys.Any_Key )
            {
                return PressedKeyCount > 0;
            }

            if ( key is < 0 or > IInput.Keys.MaxKeycode )
            {
                return false;
            }

            return PressedKeys[ key ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyJustPressed( int key )
        {
            if ( key == IInput.Keys.Any_Key )
            {
                return KeyJustPressed;
            }

            if ( key is < 0 or > IInput.Keys.MaxKeycode )
            {
                return false;
            }

            return JustPressedKeys[ key ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCatchBackKey()
        {
            return _keysToCatch.Contains( IInput.Keys.Back );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catchBack"></param>
        public void SetCatchBackKey( bool catchBack )
        {
            SetCatchKey( IInput.Keys.Back, catchBack );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCatchMenuKey()
        {
            return _keysToCatch.Contains( IInput.Keys.Menu );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catchMenu"></param>
        public void SetCatchMenuKey( bool catchMenu )
        {
            SetCatchKey( IInput.Keys.Menu, catchMenu );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keycode"></param>
        /// <param name="catchKey"></param>
        public void SetCatchKey( int keycode, bool catchKey )
        {
            if ( !catchKey )
            {
                _keysToCatch.Remove( keycode );
            }
            else
            {
                _keysToCatch.Add( keycode );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keycode"></param>
        /// <returns></returns>
        public bool IsCatchKey( int keycode )
        {
            return _keysToCatch.Contains( keycode );
        }

        public abstract float GetAccelerometerX();

        public abstract float GetAccelerometerY();

        public abstract float GetAccelerometerZ();

        public abstract float GetGyroscopeX();

        public abstract float GetGyroscopeY();

        public abstract float GetGyroscopeZ();

        public abstract int GetMaxPointers();

        public abstract int GetX( int pointer = 0 );

        public abstract int GetDeltaX( int pointer = 0 );

        public abstract int GetY( int pointer = 0 );

        public abstract int GetDeltaY( int pointer = 0 );

        public abstract bool IsTouched( int pointer = 0 );

        public abstract bool JustTouched();

        public abstract float GetPressure( int pointer = 0 );

        public abstract bool IsButtonPressed( int button );

        public abstract bool IsButtonJustPressed( int button );

        public abstract void SetInputProcessor( IInputProcessor processor );

        public abstract IInputProcessor GetInputProcessor();

        public abstract bool IsPeripheralAvailable( IInput.Peripheral peripheral );

        public abstract int GetRotation();

        public abstract IInput.Orientation GetNativeOrientation();

        public abstract void SetCursorCaught( bool caught );

        public abstract bool IsCursorCaught();

        public abstract void SetCursorPosition( int x, int y );

        public abstract void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint );

        public abstract void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint, IInput.OnscreenKeyboardType type );

        public abstract void SetOnscreenKeyboardVisible( bool visible );

        public abstract void SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type );

        public abstract void Vibrate( int milliseconds );

        public abstract void Vibrate( long[] pattern, int repeat );

        public abstract void CancelVibrate();

        public abstract float GetAzimuth();

        public abstract float GetPitch();

        public abstract float GetRoll();

        public abstract void GetRotationMatrix( float[] matrix );

        public abstract long GetCurrentEventTime();
    }
}