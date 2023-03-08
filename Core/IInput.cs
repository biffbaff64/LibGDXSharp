namespace LibGDXSharp
{
    /// <summary>
    /// Interface to the input facilities. This allows polling the state of the
    /// keyboard, the touch screen and the accelerometer. On some backends
    /// (desktop, gwt, etc) the touch screen is replaced by mouse input. The
    /// accelerometer is of course not available on all backends.
    /// Instead of polling for events, one can process all input events with an
    /// InputProcessor. You can set the InputProcessor via the SetInputProcessor(InputProcessor)
    /// method. It will be called before the ApplicationListener.render() method in each frame.
    /// Keyboard keys are translated to the constants in Input.Keys transparently on all systems.
    /// Do not use system specific key constants.
    /// The class also offers methods to use (and test for the presence of) other input systems
    /// like vibration, compass, on-screen keyboards, and cursor capture.
    /// Support for simple input dialogs is also provided.
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Mouse Buttons
        /// </summary>
        public static class Buttons
        {
            public const int Left    = 0;
            public const int Right   = 1;
            public const int Middle  = 2;
            public const int Back    = 3;
            public const int Forward = 4;
        }

        public static class Keys
        {
            public const int Any_Key             = -1;
            public const int Num_0               = 7;
            public const int Num_1               = 8;
            public const int Num_2               = 9;
            public const int Num_3               = 10;
            public const int Num_4               = 11;
            public const int Num_5               = 12;
            public const int Num_6               = 13;
            public const int Num_7               = 14;
            public const int Num_8               = 15;
            public const int Num_9               = 16;
            public const int A                   = 29;
            public const int Alt_Left            = 57;
            public const int Alt_Right           = 58;
            public const int Apostrophe          = 75;
            public const int At                  = 77;
            public const int B                   = 30;
            public const int Back                = 4;
            public const int Backslash           = 73;
            public const int C                   = 31;
            public const int Call                = 5;
            public const int Camera              = 27;
            public const int Caps_Lock           = 115;
            public const int Clear               = 28;
            public const int Comma               = 55;
            public const int D                   = 32;
            public const int Del                 = 67;
            public const int Backspace           = 67;
            public const int Forward_Del         = 112;
            public const int Dpad_Center         = 23;
            public const int Dpad_Down           = 20;
            public const int Dpad_Left           = 21;
            public const int Dpad_Right          = 22;
            public const int Dpad_Up             = 19;
            public const int Center              = 23;
            public const int Down                = 20;
            public const int Left                = 21;
            public const int Right               = 22;
            public const int Up                  = 19;
            public const int E                   = 33;
            public const int Endcall             = 6;
            public const int Enter               = 66;
            public const int Envelope            = 65;
            public const int Equals_Sign         = 70;
            public const int Explorer            = 64;
            public const int F                   = 34;
            public const int Focus               = 80;
            public const int G                   = 35;
            public const int Grave               = 68;
            public const int H                   = 36;
            public const int Headsethook         = 79;
            public const int Home                = 3;
            public const int I                   = 37;
            public const int J                   = 38;
            public const int K                   = 39;
            public const int L                   = 40;
            public const int Left_Bracket        = 71;
            public const int M                   = 41;
            public const int Media_Fast_Forward  = 90;
            public const int Media_Next          = 87;
            public const int Media_Play_Pause    = 85;
            public const int Media_Previous      = 88;
            public const int Media_Rewind        = 89;
            public const int Media_Stop          = 86;
            public const int Menu                = 82;
            public const int Minus               = 69;
            public const int Mute                = 91;
            public const int N                   = 42;
            public const int Notification        = 83;
            public const int Num                 = 78;
            public const int O                   = 43;
            public const int P                   = 44;
            public const int Pause               = 121; // aka break
            public const int Period              = 56;
            public const int Plus                = 81;
            public const int Pound               = 18;
            public const int Power               = 26;
            public const int Print_Screen        = 120; // aka SYSRQ
            public const int Q                   = 45;
            public const int R                   = 46;
            public const int Right_Bracket       = 72;
            public const int S                   = 47;
            public const int Scroll_Lock         = 116;
            public const int Search              = 84;
            public const int Semicolon           = 74;
            public const int Shift_Left          = 59;
            public const int Shift_Right         = 60;
            public const int Slash               = 76;
            public const int Soft_Left           = 1;
            public const int Soft_Right          = 2;
            public const int Space               = 62;
            public const int Star                = 17;
            public const int Sym                 = 63;
            public const int T                   = 48;
            public const int Tab                 = 61;
            public const int U                   = 49;
            public const int Unknown             = 0;
            public const int V                   = 50;
            public const int Volume_Down         = 25;
            public const int Volume_Up           = 24;
            public const int W                   = 51;
            public const int X                   = 52;
            public const int Y                   = 53;
            public const int Z                   = 54;
            public const int Meta_Alt_Left_On    = 16;
            public const int Meta_Alt_On         = 2;
            public const int Meta_Alt_Right_On   = 32;
            public const int Meta_Shift_Left_On  = 64;
            public const int Meta_Shift_On       = 1;
            public const int Meta_Shift_Right_On = 128;
            public const int Meta_Sym_On         = 4;
            public const int Control_Left        = 129;
            public const int Control_Right       = 130;
            public const int Escape              = 111;
            public const int End                 = 123;
            public const int Insert              = 124;
            public const int Page_Up             = 92;
            public const int Page_Down           = 93;
            public const int Pictsymbols         = 94;
            public const int Switch_Charset      = 95;
            public const int Button_Circle       = 255;
            public const int Button_A            = 96;
            public const int Button_B            = 97;
            public const int Button_C            = 98;
            public const int Button_X            = 99;
            public const int Button_Y            = 100;
            public const int Button_Z            = 101;
            public const int Button_L1           = 102;
            public const int Button_R1           = 103;
            public const int Button_L2           = 104;
            public const int Button_R2           = 105;
            public const int Button_Thumbl       = 106;
            public const int Button_Thumbr       = 107;
            public const int Button_Start        = 108;
            public const int Button_Select       = 109;
            public const int Button_Mode         = 110;

            public const int Numpad_0 = 144;
            public const int Numpad_1 = 145;
            public const int Numpad_2 = 146;
            public const int Numpad_3 = 147;
            public const int Numpad_4 = 148;
            public const int Numpad_5 = 149;
            public const int Numpad_6 = 150;
            public const int Numpad_7 = 151;
            public const int Numpad_8 = 152;
            public const int Numpad_9 = 153;

            public const int Numpad_Divide      = 154;
            public const int Numpad_Multiply    = 155;
            public const int Numpad_Subtract    = 156;
            public const int Numpad_Add         = 157;
            public const int Numpad_Dot         = 158;
            public const int Numpad_Comma       = 159;
            public const int Numpad_Enter       = 160;
            public const int Numpad_Equals      = 161;
            public const int Numpad_Left_Paren  = 162;
            public const int Numpad_Right_Paren = 163;
            public const int Num_Lock           = 143;

            public const int Colon = 243;
            public const int F1    = 131;
            public const int F2    = 132;
            public const int F3    = 133;
            public const int F4    = 134;
            public const int F5    = 135;
            public const int F6    = 136;
            public const int F7    = 137;
            public const int F8    = 138;
            public const int F9    = 139;
            public const int F10   = 140;
            public const int F11   = 141;
            public const int F12   = 142;
            public const int F13   = 183;
            public const int F14   = 184;
            public const int F15   = 185;
            public const int F16   = 186;
            public const int F17   = 187;
            public const int F18   = 188;
            public const int F19   = 189;
            public const int F20   = 190;
            public const int F21   = 191;
            public const int F22   = 192;
            public const int F23   = 193;
            public const int F24   = 194;

            public const int MaxKeycode = 255;

            public static string ToString( int keycode )
            {
                if ( keycode < 0 )
                {
                    throw new ArgumentException( "keycode cannot be < 0 - " + keycode );
                }

                if ( keycode > MaxKeycode )
                {
                    throw new ArgumentException( "keycode cannot be > MaxKeycode - " + keycode );
                }

                var str = keycode switch
                {
                    Unknown            => "Unknown",
                    Soft_Left          => "Soft Left",
                    Soft_Right         => "Soft Right",
                    Home               => "Home",
                    Back               => "Back",
                    Call               => "Call",
                    Endcall            => "End Call",
                    Num_0              => "0",
                    Num_1              => "1",
                    Num_2              => "2",
                    Num_3              => "3",
                    Num_4              => "4",
                    Num_5              => "5",
                    Num_6              => "6",
                    Num_7              => "7",
                    Num_8              => "8",
                    Num_9              => "9",
                    Star               => "*",
                    Pound              => "#",
                    Up                 => "Up",
                    Down               => "Down",
                    Left               => "Left",
                    Right              => "Right",
                    Center             => "Center",
                    Volume_Up          => "Volume Up",
                    Volume_Down        => "Volume Down",
                    Power              => "Power",
                    Camera             => "Camera",
                    Clear              => "Clear",
                    A                  => "A",
                    B                  => "B",
                    C                  => "C",
                    D                  => "D",
                    E                  => "E",
                    F                  => "F",
                    G                  => "G",
                    H                  => "H",
                    I                  => "I",
                    J                  => "J",
                    K                  => "K",
                    L                  => "L",
                    M                  => "M",
                    N                  => "N",
                    O                  => "O",
                    P                  => "P",
                    Q                  => "Q",
                    R                  => "R",
                    S                  => "S",
                    T                  => "T",
                    U                  => "U",
                    V                  => "V",
                    W                  => "W",
                    X                  => "X",
                    Y                  => "Y",
                    Z                  => "Z",
                    Comma              => ",",
                    Period             => ".",
                    Alt_Left           => "L-Alt",
                    Alt_Right          => "R-Alt",
                    Shift_Left         => "L-Shift",
                    Shift_Right        => "R-Shift",
                    Tab                => "Tab",
                    Space              => "Space",
                    Sym                => "SYM",
                    Explorer           => "Explorer",
                    Envelope           => "Envelope",
                    Enter              => "Enter",
                    Del                => "Delete", // also BACKSPACE
                    Grave              => "`",
                    Minus              => "-",
                    Equals_Sign        => "=",
                    Left_Bracket       => "[",
                    Right_Bracket      => "]",
                    Backslash          => "\\",
                    Semicolon          => ";",
                    Apostrophe         => "'",
                    Slash              => "/",
                    At                 => "@",
                    Num                => "Num",
                    Headsethook        => "Headset Hook",
                    Focus              => "Focus",
                    Plus               => "Plus",
                    Menu               => "Menu",
                    Notification       => "Notification",
                    Search             => "Search",
                    Media_Play_Pause   => "Play/Pause",
                    Media_Stop         => "Stop Media",
                    Media_Next         => "Next Media",
                    Media_Previous     => "Prev Media",
                    Media_Rewind       => "Rewind",
                    Media_Fast_Forward => "Fast Forward",
                    Mute               => "Mute",
                    Page_Up            => "Page Up",
                    Page_Down          => "Page Down",
                    Pictsymbols        => "PICTSYMBOLS",
                    Switch_Charset     => "SWITCH_CHARSET",
                    Button_A           => "A Button",
                    Button_B           => "B Button",
                    Button_C           => "C Button",
                    Button_X           => "X Button",
                    Button_Y           => "Y Button",
                    Button_Z           => "Z Button",
                    Button_L1          => "L1 Button",
                    Button_R1          => "R1 Button",
                    Button_L2          => "L2 Button",
                    Button_R2          => "R2 Button",
                    Button_Thumbl      => "Left Thumb",
                    Button_Thumbr      => "Right Thumb",
                    Button_Start       => "Start",
                    Button_Select      => "Select",
                    Button_Mode        => "Button Mode",
                    Forward_Del        => "Forward Delete",
                    Control_Left       => "L-Ctrl",
                    Control_Right      => "R-Ctrl",
                    Escape             => "Escape",
                    End                => "End",
                    Insert             => "Insert",
                    Numpad_0           => "Numpad 0",
                    Numpad_1           => "Numpad 1",
                    Numpad_2           => "Numpad 2",
                    Numpad_3           => "Numpad 3",
                    Numpad_4           => "Numpad 4",
                    Numpad_5           => "Numpad 5",
                    Numpad_6           => "Numpad 6",
                    Numpad_7           => "Numpad 7",
                    Numpad_8           => "Numpad 8",
                    Numpad_9           => "Numpad 9",
                    Colon              => ":",
                    F1                 => "F1",
                    F2                 => "F2",
                    F3                 => "F3",
                    F4                 => "F4",
                    F5                 => "F5",
                    F6                 => "F6",
                    F7                 => "F7",
                    F8                 => "F8",
                    F9                 => "F9",
                    F10                => "F10",
                    F11                => "F11",
                    F12                => "F12",
                    F13                => "F13",
                    F14                => "F14",
                    F15                => "F15",
                    F16                => "F16",
                    F17                => "F17",
                    F18                => "F18",
                    F19                => "F19",
                    F20                => "F20",
                    F21                => "F21",
                    F22                => "F22",
                    F23                => "F23",
                    F24                => "F24",
                    Numpad_Divide      => "Num /",
                    Numpad_Multiply    => "Num *",
                    Numpad_Subtract    => "Num -",
                    Numpad_Add         => "Num +",
                    Numpad_Dot         => "Num .",
                    Numpad_Comma       => "Num ,",
                    Numpad_Enter       => "Num Enter",
                    Numpad_Equals      => "Num =",
                    Numpad_Left_Paren  => "Num (",
                    Numpad_Right_Paren => "Num )",
                    Num_Lock           => "Num Lock",
                    Caps_Lock          => "Caps Lock",
                    Scroll_Lock        => "Scroll Lock",
                    Pause              => "Pause",
                    Print_Screen       => "Print",
                    _                  => null
                };

                return str;
            }
        }

        // TODO: LibGDX uses ObjectIntMap here.
        private static List< string > _keyNames;

        public static int ValueOf( string keyname )
        {
            if ( _keyNames == null )
            {
                _keyNames = new List< string >();

                InitialiseKeyNames();
            }

            return _keyNames.IndexOf( keyname );
        }

        private static void InitialiseKeyNames()
        {
            for ( var i = 0; i <= Keys.MaxKeycode; i++ )
            {
                var name = Keys.ToString( i );

                if ( name != null )
                {
                    _keyNames[ i ] = name;
                }
            }
        }

        public interface ITextInputListener
        {
            void Input( string text );

            void Canceled();
        }

        public enum Peripheral
        {
            HardwareKeyboard,
            OnscreenKeyboard,
            MultitouchScreen,
            Accelerometer,
            Compass,
            Vibrator,
            Gyroscope,
            RotationVector,
            Pressure
        }

        public enum OnscreenKeyboardType
        {
            Default, NumberPad, PhonePad, Email, Password, Uri
        }

        public enum Orientation
        {
            Landscape, Portrait
        }

        public float GetAccelerometerX();
        public float GetAccelerometerY();
        public float GetAccelerometerZ();
        public float GetGyroscopeX();
        public float GetGyroscopeY();
        public float GetGyroscopeZ();

        public int GetMaxPointers();
        public int GetX( int pointer = 0 );
        public int GetDeltaX( int pointer = 0 );
        public int GetY( int pointer = 0 );
        public int GetDeltaY( int pointer = 0 );

        public bool IsTouched( int pointer = 0 );
        public bool JustTouched();

        public float GetPressure( int pointer = 0 );

        public bool IsButtonPressed( int button );
        public bool IsButtonJustPressed( int button );

        public bool IsKeyPressed( int key );
        public bool IsKeyJustPressed( int key );

        public void GetTextInput( ITextInputListener listener, string title, string text, string hint );

        public void GetTextInput( ITextInputListener listener,
                                  string title,
                                  string text,
                                  string hint,
                                  OnscreenKeyboardType type );

        public void SetOnscreenKeyboardVisible( bool visible );
        public void SetOnscreenKeyboardVisible( bool visible, OnscreenKeyboardType type );

        public void Vibrate( int milliseconds );
        public void Vibrate( long[] pattern, int repeat );
        public void CancelVibrate();

        public float GetAzimuth();

        public float GetPitch();

        public float GetRoll();

        public void GetRotationMatrix( float[] matrix );

        public long GetCurrentEventTime();

        public void SetCatchKey( int keycode, bool catchKey );

        public bool IsCatchKey( int keycode );

        public void SetInputProcessor( IInputProcessor processor );

        public IInputProcessor GetInputProcessor();

        public bool IsPeripheralAvailable( Peripheral peripheral );

        public int GetRotation();

        public Orientation GetNativeOrientation();

        /// <remarks>
        /// Java LibGDX has this method named as SetCursorCatched(bool catched).
        /// </remarks>
        public void SetCursorCaught( bool caught );

        /// <remarks>
        /// Java LibGDX has this method named as IsCursorCatched().
        /// </remarks>
        public bool IsCursorCaught();

        public void SetCursorPosition( int x, int y );
    }
}
