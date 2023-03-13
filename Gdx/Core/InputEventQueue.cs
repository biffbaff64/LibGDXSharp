namespace LibGDXSharp.Core
{
    public class InputEventQueue
    {
        private const int Skip           = -1;
        private const int Key_Down       = 0;
        private const int Key_Up         = 1;
        private const int Key_Typed      = 2;
        private const int Touch_Down     = 3;
        private const int Touch_Up       = 4;
        private const int Touch_Dragged  = 5;
        private const int Mouse_Moved    = 6;
        private const int Mouse_Scrolled = 7;

        private readonly List< int > _queue           = new List< int >();
        private readonly List< int > _processingQueue = new List< int >();

        private long _currentEventTime;

        public void Drain( IInputProcessor? processor )
        {
            lock ( this )
            {
                if ( processor == null )
                {
                    _queue.Clear();

                    return;
                }

                _processingQueue.AddRange( _queue );
                _queue.Clear();
            }

            var q = _processingQueue.ToArray();

            for ( int i = 0, n = _processingQueue.Count; i < n; )
            {
                var type = q[ i++ ];
                _currentEventTime = ( ( long )q[ i++ ] << 32 ) | ( q[ i++ ] & 0xFFFFFFFFL );

                switch ( type )
                {
                    case Skip:
                        i += q[ i ];

                        break;

                    case Key_Down:
                        processor.KeyDown( q[ i++ ] );

                        break;

                    case Key_Up:
                        processor.KeyUp( q[ i++ ] );

                        break;

                    case Key_Typed:
                        processor.KeyTyped( ( char )q[ i++ ] );

                        break;

                    case Touch_Down:
                        processor.TouchDown( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                        break;

                    case Touch_Up:
                        processor.TouchUp( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                        break;

                    case Touch_Dragged:
                        processor.TouchDragged( q[ i++ ], q[ i++ ], q[ i++ ] );

                        break;

                    case Mouse_Moved:
                        processor.MouseMoved( q[ i++ ], q[ i++ ] );

                        break;

                    case Mouse_Scrolled:
                        processor.Scrolled( NumberUtils.IntBitsToFloat( q[ i++ ] ), NumberUtils.IntBitsToFloat( q[ i++ ] ) );

                        break;

                    default:
                        throw new SystemException();
                }
            }

            _processingQueue.Clear();
        }

        private int Next( int nextType, int i )
        {
            lock ( this )
            {
                var q = _queue.ToArray();

                for ( var n = _queue.Count; i < n; )
                {
                    var type = q[ i ];

                    if ( type == nextType ) return i;

                    i += 3;

                    switch ( type )
                    {
                        case Skip:
                            i += q[ i ];

                            break;

                        case Key_Down:
                            i++;

                            break;

                        case Key_Up:
                            i++;

                            break;

                        case Key_Typed:
                            i++;

                            break;

                        case Touch_Down:
                            i += 4;

                            break;

                        case Touch_Up:
                            i += 4;

                            break;

                        case Touch_Dragged:
                            i += 3;

                            break;

                        case Mouse_Moved:
                            i += 2;

                            break;

                        case Mouse_Scrolled:
                            i += 2;

                            break;

                        default:
                            throw new SystemException();
                    }
                }
            }

            return -1;
        }

        private void QueueTime( long time )
        {
            _queue.Add( ( int )( time >> 32 ) );
            _queue.Add( ( int )time );
        }

        public bool KeyDown( int keycode, long time )
        {
            lock ( this )
            {
                _queue.Add( Key_Down );
                QueueTime( time );
                _queue.Add( keycode );
            }

            return false;
        }

        public bool KeyUp( int keycode, long time )
        {
            lock ( this )
            {
                _queue.Add( Key_Up );

                QueueTime( time );

                _queue.Add( keycode );
            }

            return false;
        }

        public bool KeyTyped( char character, long time )
        {
            lock ( this )
            {
                _queue.Add( Key_Typed );
                QueueTime( time );
                _queue.Add( character );
            }

            return false;
        }

        public bool TouchDown( int screenX, int screenY, int pointer, int button, long time )
        {
            lock ( this )
            {
                _queue.Add( Touch_Down );

                QueueTime( time );

                _queue.Add( screenX );
                _queue.Add( screenY );
                _queue.Add( pointer );
                _queue.Add( button );
            }

            return false;
        }

        public bool TouchUp( int screenX, int screenY, int pointer, int button, long time )
        {
            lock ( this )
            {
                _queue.Add( Touch_Up );

                QueueTime( time );

                _queue.Add( screenX );
                _queue.Add( screenY );
                _queue.Add( pointer );
                _queue.Add( button );
            }

            return false;
        }

        public bool TouchDragged( int screenX, int screenY, int pointer, long time )
        {
            lock ( this )
            {
                // Skip any queued touch dragged events for the same pointer.
                for ( var i = Next( Touch_Dragged, 0 ); i >= 0; i = Next( Touch_Dragged, i + 6 ) )
                {
                    if ( _queue[ i + 5 ] == pointer )
                    {
                        _queue[ i ]     = Skip;
                        _queue[ i + 3 ] = 3;
                    }
                }

                _queue.Add( Touch_Dragged );

                QueueTime( time );

                _queue.Add( screenX );
                _queue.Add( screenY );
                _queue.Add( pointer );
            }

            return false;
        }

        public bool MouseMoved( int screenX, int screenY, long time )
        {
            lock ( this )
            {
                // Skip any queued mouse moved events.
                for ( var i = Next( Mouse_Moved, 0 ); i >= 0; i = Next( Mouse_Moved, i + 5 ) )
                {
                    _queue[ i ]     = Skip;
                    _queue[ i + 3 ] = 2;
                }

                _queue.Add( Mouse_Moved );

                QueueTime( time );

                _queue.Add( screenX );
                _queue.Add( screenY );
            }

            return false;
        }

        public bool Scrolled( float amountX, float amountY, long time )
        {
            lock ( this )
            {
                _queue.Add( Mouse_Scrolled );

                QueueTime( time );

                _queue.Add( NumberUtils.FloatToIntBits( amountX ) );
                _queue.Add( NumberUtils.FloatToIntBits( amountY ) );
            }

            return false;
        }

        public long GetCurrentEventTime()
        {
            return _currentEventTime;
        }
    }
}
