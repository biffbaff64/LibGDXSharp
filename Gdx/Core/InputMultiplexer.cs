using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Core
{
    public class InputMultiplexer : IInputProcessor
    {
        private readonly SnapshotArray< IInputProcessor > _processors;

        public InputMultiplexer()
        {
            _processors = new SnapshotArray< IInputProcessor >( 4 );
        }

        public InputMultiplexer( params IInputProcessor[] processors )
        {
            _processors = new SnapshotArray< IInputProcessor >();
            _processors.AddAll( processors );
        }

        public void AddProcessor( int index, IInputProcessor processor )
        {
            if ( processor == null )
            {
                throw new NullReferenceException( "processor cannot be null" );
            }

            _processors.Insert( index, processor );
        }

        public void RemoveProcessor( int index )
        {
            _processors.RemoveAt( index );
        }

        public void AddProcessor( IInputProcessor processor )
        {
            if ( processor == null )
            {
                throw new NullReferenceException( "processor cannot be null" );
            }

            _processors.Add( processor );
        }

        public void RemoveProcessor( IInputProcessor processor )
        {
            _processors.Remove( processor );
        }

        public int Size()
        {
            return _processors.Count;
        }

        public void Clear()
        {
            _processors.Clear();
        }

        public void SetProcessors( params IInputProcessor[] processorList )
        {
            this._processors.Clear();
            this._processors.AddAll( processorList );
        }

        public void SetProcessors( Array< IInputProcessor > processorList )
        {
            this._processors.Clear();
            this._processors.AddAll( processorList.ToArray() );
        }

        public SnapshotArray< IInputProcessor > GetProcessors()
        {
            return _processors;
        }

        public bool KeyDown( int keycode )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).KeyDown( keycode ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool KeyUp( int keycode )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).KeyUp( keycode ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool KeyTyped( char character )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).KeyTyped( character ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool TouchDown( int screenX, int screenY, int pointer, int button )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).TouchDown( screenX, screenY, pointer, button ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool TouchUp( int screenX, int screenY, int pointer, int button )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).TouchUp( screenX, screenY, pointer, button ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool TouchDragged( int screenX, int screenY, int pointer )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).TouchDragged( screenX, screenY, pointer ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool MouseMoved( int screenX, int screenY )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).MouseMoved( screenX, screenY ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }

        public bool Scrolled( float amountX, float amountY )
        {
            var items = _processors.Begin();

            try
            {
                for ( int i = 0, n = _processors.Size; i < n; i++ )
                {
                    if ( ( ( IInputProcessor )items[ i ] ).Scrolled( amountX, amountY ) )
                    {
                        return true;
                    }
                }
            }
            finally
            {
                _processors.End();
            }

            return false;
        }
    }
}
