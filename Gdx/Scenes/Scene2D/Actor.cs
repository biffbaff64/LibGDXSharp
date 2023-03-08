using System.Numerics;

using LibGDXSharp.G2D;
using LibGDXSharp.Math;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D
{
    public class Actor
    {
        public Stage? Stage  { get; set; }
        public Group? Parent { get; set; }

        public DelayedRemovalArray< IEventListener > Listeners        { get; set; } = new DelayedRemovalArray< IEventListener >( 0 );
        public DelayedRemovalArray< IEventListener > CaptureListeners { get; set; } = new DelayedRemovalArray< IEventListener >( 0 );

        private Array< Action > _actions   = new Array< Action >( 0 );
        private Touchable       _touchable = Touchable.Enabled;
        private bool            _visible   = true;
        private bool            _debug;
        private Color           _color = new Color( 1, 1, 1, 1 );
        private string          _name;
        private float           _x;
        private float           _y;
        private float           _width;
        private float           _height;
        private float           _originX;
        private float           _originY;
        private float           _scaleX = 1;
        private float           _scaleY = 1;
        private float           _rotation;
        private object          _userObject;

        /// <summary>
        /// Draws the actor. The batch is configured to draw in the parent's coordinate
        /// system. This draw method is convenient to draw a rotated and scaled TextureRegion.
        /// Batch.begin() has already been called on the batch. If Batch.end() is called to
        /// draw without the batch then Batch.begin() must be called before the method returns.
        /// The default implementation does nothing.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="parentAlpha">
        /// The parent alpha, to be multiplied with this actor's alpha,
        /// allowing the parent's alpha to affect all children.
        /// </param>
        public virtual void Draw( IBatch batch, float parentAlpha )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <exception cref="SystemException"></exception>
        public void Act( float delta )
        {
            if ( _actions.Count == 0 ) return;

            if ( ( Stage != null ) && Stage.GetActionsRequestRendering() )
            {
                Gdx.Graphics.RequestRendering();
            }

            try
            {
                for ( var i = 0; i < _actions.Count; i++ )
                {
                    var action = _actions[ i ];

                    if ( _actions[ i ].Act( delta ) && i < _actions.Count )
                    {
                        var current     = _actions[ i ];
                        var actionIndex = current == action ? i : _actions.IndexOf( action );

                        if ( actionIndex != -1 )
                        {
                            _actions.RemoveAt( actionIndex );
                            _actions[ i ].SetActor( null );
                            i--;
                        }
                    }
                }
            }
            catch ( SystemException ex )
            {
                var context = ToString();

                if ( context != null )
                {
                    throw new SystemException
                        (
                         string.Concat( "Actor - ", context.AsSpan( 0, System.Math.Min( context.Length, 128 ) ) ),
                         ex
                        );
                }
            }
        }

        /// <summary>
        /// Sets this actor as the event target and propagates the event to
        /// this actor and ascendants as necessary. If this actor is not in
        /// the stage, the stage must be set before calling this method.
        /// Events are fired in 2 phases:
        /// 
        /// 1.  The first phase (the "capture" phase) notifies listeners on
        ///     each actor starting at the root and propagating down the
        ///     hierarchy to (and including) this actor.
        /// 2.  The second phase notifies listeners on each actor starting
        ///     at this actor and, if Event.getBubbles() is true, propagating
        ///     upward to the root.
        /// 
        /// If the event is stopped at any time, it will not propagate to the
        /// next actor.
        /// </summary>
        /// <param name="ev">The <see cref="Event"/></param>
        /// <returns>True if the event was cancelled.</returns>
        public bool Fire( Event ev )
        {
            if ( ev.Stage == null )
            {
                ev.Stage = this.Stage;
            }

            ev.TargetActor = this;

            // Collect ascendants so event propagation is unaffected by
            // hierarchy changes.
            var ascendants = Pools.Obtain( typeof(Array) );
            var parent     = this.Parent;

            while ( parent != null )
            {
                ascendants.add( parent );
                parent = parent.Parent;
            }

            try
            {
                // Notify ascendants' capture listeners, starting at the root. Ascendants may stop an event before children receive it.
                var ascendantsArray = ascendants.items;

                for ( var i = ascendants.size - 1; i >= 0; i-- )
                {
                    var currentTarget = ( Group )ascendantsArray[ i ];

                    currentTarget.Notify( ev, true );

                    if ( ev.IsStopped ) return ev.IsCancelled;
                }

                // Notify the target capture listeners.
                Notify( ev, true );

                if ( ev.IsStopped ) return ev.IsCancelled;

                // Notify the target listeners.
                Notify( ev, false );

                if ( !ev.Bubbles ) return ev.IsCancelled;
                if ( ev.IsStopped ) return ev.IsCancelled;

                // Notify ascendants' actor listeners, starting at the target.
                // Children may stop an event before ascendants receive it.
                for ( int i = 0, n = ascendants.size; i < n; i++ )
                {
                    ( ( Group )ascendantsArray[ i ] ).Notify( ev, false );

                    if ( ev.IsStopped ) return ev.IsCancelled;
                }

                return ev.IsCancelled;
            }
            finally
            {
                ascendants.clear();

                Pools.Free( ascendants );
            }
        }

        public bool Notify( Event ev, bool capture )
        {
            if ( ev.TargetActor == null )
            {
                throw new ArgumentException( "The event target cannot be null." );
            }

            var listeners = capture ? CaptureListeners : this.Listeners;

            if ( listeners.Count == 0 ) return ev.IsCancelled;

            ev.ListenerActor = this;
            ev.Capture       = capture;

            ev.Stage ??= this.Stage;

            try
            {
                listeners.Begin();

                for ( int i = 0, n = listeners.Count; i < n; i++ )
                {
                    if ( listeners[ i ].Handle( ev ) )
                    {
                        ev.IsHandled = true;
                    }
                }

                listeners.End();
            }
            catch ( SystemException ex )
            {
                var context = ToString();

                if ( context != null )
                {
                    throw new SystemException
                        (
                         string.Concat( "Actor - ", context.AsSpan( 0, System.Math.Min( context.Length, 128 ) ) ),
                         ex
                        );
                }
            }

            return ev.IsCancelled;
        }

        /// <summary>
        /// Returns the deepest visible (and optionally, touchable) actor that
        /// contains the specified point, or null if no actor was hit. The point
        /// is specified in the actor's local coordinate system (0,0 is the bottom
        /// left of the actor and width,height is the upper right).
        /// This method is used to delegate touchDown, mouse, and enter/exit events.
        /// If this method returns null, those events will not occur on this Actor.
        /// The default implementation returns this actor if the point is within
        /// this actor's bounds and this actor is visible.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="touchable"></param>
        /// <returns></returns>
        public Actor? Hit( float x, float y, bool touchable )
        {
            if ( touchable && this._touchable != Touchable.Enabled ) return null;

            if ( !_visible ) return null;

            return ( ( x >= 0 ) && ( x < _width ) && ( y >= 0 ) && ( y < _height ) ) ? this : null;
        }

        /// <summary>
        /// Removes this actor from its parent, if it has a parent.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool Remove()
        {
            if ( Parent != null ) return Parent.RemoveActor( this, true );

            return false;
        }

        /// <summary>
        /// Add a listener to receive events that hit this actor.
        /// </summary>
        public bool AddListener( IEventListener listener )
        {
            if ( listener == null ) throw new ArgumentException( "listener cannot be null." );

            if ( !Listeners.Contains( listener ) )
            {
                Listeners.Add( listener );

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool RemoveListener( IEventListener listener )
        {
            if ( listener == null ) throw new ArgumentException( "listener cannot be null." );

            return Listeners.Remove( listener );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool AddCaptureListener( IEventListener listener )
        {
            if ( listener == null ) throw new ArgumentException( "listener cannot be null." );

            if ( !CaptureListeners.Contains( listener ) ) CaptureListeners.Add( listener );

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool RemoveCaptureListener( IEventListener listener )
        {
            if ( listener == null ) throw new ArgumentException( "listener cannot be null." );

            return CaptureListeners.Remove( listener );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void AddAction( Action action )
        {
            action.SetActor( this );

            _actions.Add( action );

            if ( Stage != null && Stage.GetActionsRequestRendering() ) Gdx.Graphics.RequestRendering();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction( Action? action )
        {
            if ( ( action != null ) && _actions.Remove( action ) ) action.SetActor( null );
        }

        /// <summary>
        /// Returns true if the actor has one or more actions.
        /// </summary>
        public bool HasActions()
        {
            return _actions.Count > 0;
        }

        /// <summary>
        /// Removes all actions on this actor.
        /// </summary>
        public void ClearActions()
        {
            for ( var i = _actions.Count - 1; i >= 0; i-- )
            {
                _actions[ i ].SetActor( null );
            }

            _actions.Clear();
        }

        /// <summary>
        /// Removes all listeners on this actor.
        /// </summary>
        public void ClearListeners()
        {
            Listeners.Clear();
            CaptureListeners.Clear();
        }

        /** Removes all actions and listeners on this actor. */
        public void Clear()
        {
            ClearActions();
            ClearListeners();
        }

        /// <summary>
        /// Returns true if this actor is the same as or is the descendant
        /// of the specified actor.
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsDescendantOf( Actor actor )
        {
            if ( actor == null ) throw new ArgumentException( "actor cannot be null." );

            var parent = this;

            do
            {
                if ( parent == actor ) return true;

                parent = parent.Parent;
            }
            while ( parent != null );

            return false;
        }


        /** Returns true if this actor is the same as or is the ascendant of the specified actor. */
        public bool isAscendantOf( Actor actor )
        {
            if ( actor == null ) throw new IllegalArgumentException( "actor cannot be null." );

            do
            {
                if ( actor == this ) return true;
                actor = actor.parent;
            }
            while ( actor != null );

            return false;
        }

        /** Returns this actor or the first ascendant of this actor that is assignable with the specified type, or null if none were
	 * found. */
        public @Null< T extends Actor> T firstAscendant( Class< T > type )
        {
            if ( type == null ) throw new IllegalArgumentException( "actor cannot be null." );
            Actor actor = this;

            do
            {
                if ( ClassReflection.isInstance( type, actor ) ) return ( T )actor;
                actor = actor.parent;
            }
            while ( actor != null );

            return null;
        }

        /** Returns true if the actor's parent is not null. */
        public bool hasParent()
        {
            return parent != null;
        }

        /** Returns the parent actor, or null if not in a group. */
        public @Null Group getParent()
        {
            return parent;
        }

        /** Called by the framework when an actor is added to or removed from a group.
	 * @param parent May be null if the actor has been removed from the parent. */
        protected void setParent( @Null Group parent )
        {
            this.parent = parent;
        }

        /** Returns true if input events are processed by this actor. */
        public bool isTouchable()
        {
            return touchable == Touchable.enabled;
        }

        public Touchable getTouchable()
        {
            return touchable;
        }

        /** Determines how touch events are distributed to this actor. Default is {@link Touchable#enabled}. */
        public void setTouchable( Touchable touchable )
        {
            this.touchable = touchable;
        }

        public bool isVisible()
        {
            return visible;
        }

        /** If false, the actor will not be drawn and will not receive touch events. Default is true. */
        public void setVisible( bool visible )
        {
            this.visible = visible;
        }

        /** Returns true if this actor and all ascendants are visible. */
        public bool ascendantsVisible()
        {
            Actor actor = this;

            do
            {
                if ( !actor.isVisible() ) return false;
                actor = actor.parent;
            }
            while ( actor != null );

            return true;
        }

        /** @deprecated Use {@link #ascendantsVisible()}. */
        @Deprecated

        public bool ancestorsVisible()
        {
            return ascendantsVisible();
        }

        /** Returns true if this actor is the {@link Stage#getKeyboardFocus() keyboard focus} actor. */
        public bool hasKeyboardFocus()
        {
            Stage stage = getStage();

            return stage != null && stage.getKeyboardFocus() == this;
        }

        /** Returns true if this actor is the {@link Stage#getScrollFocus() scroll focus} actor. */
        public bool hasScrollFocus()
        {
            Stage stage = getStage();

            return stage != null && stage.getScrollFocus() == this;
        }

        /** Returns true if this actor is a target actor for touch focus.
	 * @see Stage#addTouchFocus(EventListener, Actor, Actor, int, int) */
        public bool isTouchFocusTarget()
        {
            Stage stage = getStage();

            if ( stage == null ) return false;

            for ( int i = 0, n = stage.touchFocuses.size; i < n; i++ )
                if ( stage.touchFocuses.get( i ).target == this )
                    return true;

            return false;
        }

        /** Returns true if this actor is a listener actor for touch focus.
	 * @see Stage#addTouchFocus(EventListener, Actor, Actor, int, int) */
        public bool isTouchFocusListener()
        {
            Stage stage = getStage();

            if ( stage == null ) return false;

            for ( int i = 0, n = stage.touchFocuses.size; i < n; i++ )
                if ( stage.touchFocuses.get( i ).listenerActor == this )
                    return true;

            return false;
        }

        /** Returns an application specific object for convenience, or null. */
        public @Null Object getUserObject()
        {
            return userObject;
        }

        /** Sets an application specific object for convenience. */
        public void setUserObject( @Null Object userObject )
        {
            this.userObject = userObject;
        }

        /** Returns the X position of the actor's left edge. */
        public float getX()
        {
            return x;
        }

        /** Returns the X position of the specified {@link Align alignment}. */
        public float getX( int alignment )
        {
            float x = this.x;

            if ( ( alignment & right ) != 0 )
                x += width;
            else if ( ( alignment & left ) == 0 ) //
                x += width / 2;

            return x;
        }

        public void setX( float x )
        {
            if ( this.x != x )
            {
                this.x = x;
                positionChanged();
            }
        }

        /** Sets the x position using the specified {@link Align alignment}. Note this may set the position to non-integer
	 * coordinates. */
        public void setX( float x, int alignment )
        {

            if ( ( alignment & right ) != 0 )
                x -= width;
            else if ( ( alignment & left ) == 0 ) //
                x -= width / 2;

            if ( this.x != x )
            {
                this.x = x;
                positionChanged();
            }
        }

        /** Returns the Y position of the actor's bottom edge. */
        public float getY()
        {
            return y;
        }

        public void setY( float y )
        {
            if ( this.y != y )
            {
                this.y = y;
                positionChanged();
            }
        }

        /** Sets the y position using the specified {@link Align alignment}. Note this may set the position to non-integer
	 * coordinates. */
        public void setY( float y, int alignment )
        {

            if ( ( alignment & top ) != 0 )
                y -= height;
            else if ( ( alignment & bottom ) == 0 ) //
                y -= height / 2;

            if ( this.y != y )
            {
                this.y = y;
                positionChanged();
            }
        }

        /** Returns the Y position of the specified {@link Align alignment}. */
        public float getY( int alignment )
        {
            float y = this.y;

            if ( ( alignment & top ) != 0 )
                y += height;
            else if ( ( alignment & bottom ) == 0 ) //
                y += height / 2;

            return y;
        }

        /** Sets the position of the actor's bottom left corner. */
        public void setPosition( float x, float y )
        {
            if ( this.x != x || this.y != y )
            {
                this.x = x;
                this.y = y;
                positionChanged();
            }
        }

        /** Sets the position using the specified {@link Align alignment}. Note this may set the position to non-integer
	 * coordinates. */
        public void setPosition( float x, float y, int alignment )
        {
            if ( ( alignment & right ) != 0 )
                x -= width;
            else if ( ( alignment & left ) == 0 ) //
                x -= width / 2;

            if ( ( alignment & top ) != 0 )
                y -= height;
            else if ( ( alignment & bottom ) == 0 ) //
                y -= height / 2;

            if ( this.x != x || this.y != y )
            {
                this.x = x;
                this.y = y;
                positionChanged();
            }
        }

        /** Add x and y to current position */
        public void moveBy( float x, float y )
        {
            if ( x != 0 || y != 0 )
            {
                this.x += x;
                this.y += y;
                positionChanged();
            }
        }

        public float getWidth()
        {
            return width;
        }

        public void setWidth( float width )
        {
            if ( this.width != width )
            {
                this.width = width;
                sizeChanged();
            }
        }

        public float getHeight()
        {
            return height;
        }

        public void setHeight( float height )
        {
            if ( this.height != height )
            {
                this.height = height;
                sizeChanged();
            }
        }

        /** Returns y plus height. */
        public float getTop()
        {
            return y + height;
        }

        /** Returns x plus width. */
        public float getRight()
        {
            return x + width;
        }

        /** Called when the actor's position has been changed. */
        protected void positionChanged()
        {
        }

        /** Called when the actor's size has been changed. */
        protected void sizeChanged()
        {
        }

        /** Called when the actor's scale has been changed. */
        protected void scaleChanged()
        {
        }

        /** Called when the actor's rotation has been changed. */
        protected void rotationChanged()
        {
        }

        /** Sets the width and height. */
        public void setSize( float width, float height )
        {
            if ( this.width != width || this.height != height )
            {
                this.width  = width;
                this.height = height;
                sizeChanged();
            }
        }

        /** Adds the specified size to the current size. */
        public void sizeBy( float size )
        {
            if ( size != 0 )
            {
                width  += size;
                height += size;
                sizeChanged();
            }
        }

        /** Adds the specified size to the current size. */
        public void sizeBy( float width, float height )
        {
            if ( width != 0 || height != 0 )
            {
                this.width  += width;
                this.height += height;
                sizeChanged();
            }
        }

        /** Set bounds the x, y, width, and height. */
        public void setBounds( float x, float y, float width, float height )
        {
            if ( this.x != x || this.y != y )
            {
                this.x = x;
                this.y = y;
                positionChanged();
            }

            if ( this.width != width || this.height != height )
            {
                this.width  = width;
                this.height = height;
                sizeChanged();
            }
        }

        public float getOriginX()
        {
            return originX;
        }

        public void setOriginX( float originX )
        {
            this.originX = originX;
        }

        public float getOriginY()
        {
            return originY;
        }

        public void setOriginY( float originY )
        {
            this.originY = originY;
        }

        /** Sets the origin position which is relative to the actor's bottom left corner. */
        public void setOrigin( float originX, float originY )
        {
            this.originX = originX;
            this.originY = originY;
        }

        /** Sets the origin position to the specified {@link Align alignment}. */
        public void setOrigin( int alignment )
        {
            if ( ( alignment & left ) != 0 )
                originX = 0;
            else if ( ( alignment & right ) != 0 )
                originX = width;
            else
                originX = width / 2;

            if ( ( alignment & bottom ) != 0 )
                originY = 0;
            else if ( ( alignment & top ) != 0 )
                originY = height;
            else
                originY = height / 2;
        }

        public float getScaleX()
        {
            return scaleX;
        }

        public void setScaleX( float scaleX )
        {
            if ( this.scaleX != scaleX )
            {
                this.scaleX = scaleX;
                scaleChanged();
            }
        }

        public float getScaleY()
        {
            return scaleY;
        }

        public void setScaleY( float scaleY )
        {
            if ( this.scaleY != scaleY )
            {
                this.scaleY = scaleY;
                scaleChanged();
            }
        }

        /** Sets the scale for both X and Y */
        public void setScale( float scaleXY )
        {
            if ( this.scaleX != scaleXY || this.scaleY != scaleXY )
            {
                this.scaleX = scaleXY;
                this.scaleY = scaleXY;
                scaleChanged();
            }
        }

        /** Sets the scale X and scale Y. */
        public void setScale( float scaleX, float scaleY )
        {
            if ( this.scaleX != scaleX || this.scaleY != scaleY )
            {
                this.scaleX = scaleX;
                this.scaleY = scaleY;
                scaleChanged();
            }
        }

        /** Adds the specified scale to the current scale. */
        public void scaleBy( float scale )
        {
            if ( scale != 0 )
            {
                scaleX += scale;
                scaleY += scale;
                scaleChanged();
            }
        }

        /** Adds the specified scale to the current scale. */
        public void scaleBy( float scaleX, float scaleY )
        {
            if ( scaleX != 0 || scaleY != 0 )
            {
                this.scaleX += scaleX;
                this.scaleY += scaleY;
                scaleChanged();
            }
        }

        public float getRotation()
        {
            return rotation;
        }

        public void setRotation( float degrees )
        {
            if ( this.rotation != degrees )
            {
                this.rotation = degrees;
                rotationChanged();
            }
        }

        /** Adds the specified rotation to the current rotation. */
        public void rotateBy( float amountInDegrees )
        {
            if ( amountInDegrees != 0 )
            {
                rotation = ( rotation + amountInDegrees ) % 360;
                rotationChanged();
            }
        }

        public void setColor( Color color )
        {
            this.color.set( color );
        }

        public void setColor( float r, float g, float b, float a )
        {
            color.set( r, g, b, a );
        }

        /** Returns the color the actor will be tinted when drawn. The returned instance can be modified to change the color. */
        public Color getColor()
        {
            return color;
        }

        /** @see #setName(string)
	 * @return May be null. */
        public @Null string getName()
        {
            return name;
        }

        /** Set the actor's name, which is used for identification convenience and by {@link #toString()}.
	 * @param name May be null.
	 * @see Group#findActor(string) */
        public void setName( @Null string name )
        {
            this.name = name;
        }

        /** Changes the z-order for this actor so it is in front of all siblings. */
        public void toFront()
        {
            SetZIndex( Integer.MAX_VALUE );
        }

        /** Changes the z-order for this actor so it is in back of all siblings. */
        public void toBack()
        {
            SetZIndex( 0 );
        }

        /** Sets the z-index of this actor. The z-index is the index into the parent's {@link Group#getChildren() children}, where a
	     * lower index is below a higher index. Setting a z-index higher than the number of children will move the child to the front.
	     * Setting a z-index less than zero is invalid.
	     * @return true if the z-index changed. */
        public bool SetZIndex( int index )
        {
            if ( index < 0 ) throw new ArgumentException( "ZIndex cannot be < 0." );
            Group parent = this.parent;

            if ( parent == null ) return false;
            Array< Actor > children = parent.children;

            if ( children.size <= 1 ) return false;
            index = Math.min( index, children.size - 1 );

            if ( children.get( index ) == this ) return false;
            if ( !children.removeValue( this, true ) ) return false;
            children.insert( index, this );

            return true;
        }

        /**
         * 
    	 * @see #setZIndex(int)
         */
        /// <summary>
        /// Returns the z-index of this actor, or -1 if the actor is not in a group.
        /// </summary>
        /// <seealso cref="SetZIndex"/>
        /// <returns></returns>
        public int GetZIndex()
        {
            if ( Parent == null ) return -1;

            return Parent.Children.IndexOf( this, true );
        }

        /** Calls {@link #clipBegin(float, float, float, float)} to clip this actor's bounds. */
        public bool ClipBegin()
        {
            return ClipBegin( _x, _y, _width, _height );
        }

        /**
         * Clips the specified screen aligned rectangle, specified relative to the transform matrix of the stage's Batch. The
	     * transform matrix and the stage's camera must not have rotational components. Calling this method must be followed by a call
	     * to {@link #clipEnd()} if true is returned.
	     * @return false if the clipping area is zero and no drawing should occur.
	     * @see ScissorStack
         */
        public bool ClipBegin( float x, float y, float width, float height )
        {
            if ( width <= 0 || height <= 0 ) return false;

            Stage? stage = this.Stage;

            if ( stage == null ) return false;

            var tableBounds = Rectangle.Tmp;
            tableBounds.X      = x;
            tableBounds.Y      = y;
            tableBounds.Width  = width;
            tableBounds.Height = height;

            Rectangle scissorBounds = Pools.Obtain( typeof(Rectangle) );

            stage.CalculateScissors( tableBounds, scissorBounds );

            if ( ScissorStack.PushScissors( scissorBounds ) ) return true;
            
            Pools.free( scissorBounds );

            return false;
        }

        /** Ends clipping begun by {@link #clipBegin(float, float, float, float)}. */
        public void ClipEnd()
        {
            Pools.Free( ScissorStack.PopScissors() );
        }

        /** Transforms the specified point in screen coordinates to the actor's local coordinate system.
    	 * @see Stage#screenToStageCoordinates(Vector2) */
        public Vector2 ScreenToLocalCoordinates( Vector2 screenCoords )
        {
            Stage? stage = this.Stage;

            if ( stage == null ) return screenCoords;

            return StageToLocalCoordinates( stage.ScreenToStageCoordinates( screenCoords ) );
        }

        /** Transforms the specified point in the stage's coordinates to the actor's local coordinate system. */
        public Vector2 StageToLocalCoordinates( Vector2 stageCoords )
        {
            if ( Parent != null ) Parent.StageToLocalCoordinates( stageCoords );

            ParentToLocalCoordinates( stageCoords );

            return stageCoords;
        }

        /** Converts the coordinates given in the parent's coordinate system to this actor's coordinate system. */
        public Vector2 ParentToLocalCoordinates( Vector2 parentCoords )
        {
            var rotation = this._rotation;
            var scaleX   = this._scaleX;
            var scaleY   = this._scaleY;
            var childX   = _x;
            var childY   = _y;

            if ( rotation == 0 )
            {
                if ( scaleX == 1 && scaleY == 1 )
                {
                    parentCoords.X -= childX;
                    parentCoords.Y -= childY;
                }
                else
                {
                    var originX = this._originX;
                    var originY = this._originY;
                    parentCoords.X = ( parentCoords.X - childX - originX ) / scaleX + originX;
                    parentCoords.Y = ( parentCoords.Y - childY - originY ) / scaleY + originY;
                }
            }
            else
            {
                var cos     = ( float )System.Math.Cos( rotation * MathUtils.DegreesToRadians );
                var sin     = ( float )System.Math.Sin( rotation * MathUtils.DegreesToRadians );
                var originX = this._originX;
                var originY = this._originY;
                var tox     = parentCoords.X - childX - originX;
                var toy     = parentCoords.Y - childY - originY;

                parentCoords.X = ( tox * cos + toy * sin ) / scaleX + originX;
                parentCoords.Y = ( tox * -sin + toy * cos ) / scaleY + originY;
            }

            return parentCoords;
        }

        /**
         * Transforms the specified point in the actor's coordinates to be in screen coordinates.
    	 * @see Stage#stageToScreenCoordinates(Vector2)
         */
        public Vector2 LocalToScreenCoordinates( Vector2 localCoords )
        {
            var stage = this.Stage;

            if ( stage == null ) return localCoords;

            return stage.StageToScreenCoordinates( LocalToAscendantCoordinates( null, localCoords ) );
        }

        /// <system>
        /// Transforms the specified point in the actor's coordinates
        /// to be in the stage's coordinates.
        /// </system>
        public Vector2 LocalToStageCoordinates( Vector2 localCoords )
        {
            return LocalToAscendantCoordinates( null, localCoords );
        }

        /// <system>
        /// Transforms the specified point in the actor's coordinates
        /// to be in the parent's coordinates.
        /// </system>
        public Vector2 LocalToParentCoordinates( Vector2 localCoords )
        {
            var rotation = -this._rotation;
            var scaleX   = this._scaleX;
            var scaleY   = this._scaleY;
            var x        = this._x;
            var y        = this._y;

            if ( rotation == 0 )
            {
                if ( ( System.Math.Abs( scaleX - 1f ) < 0.001f )
                     && ( System.Math.Abs( scaleY - 1f ) < 0.001f ) )
                {
                    localCoords.X += x;
                    localCoords.Y += y;
                }
                else
                {
                    var originX = this._originX;
                    var originY = this._originY;

                    localCoords.X = ( localCoords.X - originX ) * scaleX + originX + x;
                    localCoords.Y = ( localCoords.Y - originY ) * scaleY + originY + y;
                }
            }
            else
            {
                var cos     = ( float )System.Math.Cos( rotation * MathUtils.DegreesToRadians );
                var sin     = ( float )System.Math.Sin( rotation * MathUtils.DegreesToRadians );
                var originX = this._originX;
                var originY = this._originY;
                var tox     = ( localCoords.X - originX ) * scaleX;
                var toy     = ( localCoords.Y - originY ) * scaleY;

                localCoords.X = ( tox * cos + toy * sin ) + originX + x;
                localCoords.Y = ( tox * -sin + toy * cos ) + originY + y;
            }

            return localCoords;
        }

        /// <summary>
        /// Converts coordinates for this actor to those of an ascendant.
        /// The ascendant is not required to be the immediate parent.
        /// </summary>
        /// <param name="ascendant"></param>
        /// <param name="localCoords"></param>
        /// <returns></returns>
        public Vector2 LocalToAscendantCoordinates( Actor? ascendant, Vector2 localCoords )
        {
            var actor = this;

            do
            {
                actor.LocalToParentCoordinates( localCoords );

                actor = actor.Parent;

                if ( actor == ascendant ) break;
            }
            while ( actor != null );

            return localCoords;
        }

        /** Converts coordinates for this actor to those of another actor, which can be anywhere in the stage. */
        public Vector2 LocalToActorCoordinates( Actor actor, Vector2 localCoords )
        {
            LocalToStageCoordinates( localCoords );

            return Actor.StageToLocalCoordinates( localCoords );
        }

        /// <summary>
        /// Draws this actor's debug lines if <see cref="DebugFlag"/> is true.
        /// </summary>
        public void DrawDebug( ShapeRenderer shapes )
        {
            DrawDebugBounds( shapes );
        }

        /** Draws a rectangle for the bounds of this actor if {@link #getDebug()} is true. */
        protected void DrawDebugBounds( ShapeRenderer shapes )
        {
            if ( !DebugFlag ) return;

            // TODO: ShapeRenderer needs implementing
            shapes.Set( ShapeType.Line );

            if ( Stage != null )
            {
                shapes.SetColor( Stage.GetDebugColor() );
            }

            shapes.Rect( _x, _y, _originX, _originY, _width, _height, _scaleX, _scaleY, _rotation );
        }

        /// <summary>
        /// If true, <see cref="DrawDebug(ShapeRenderer)"/> will be called for this actor.
        /// </summary>
        public bool DebugFlag
        {
            get => _debug;
            set
            {
                _debug = value;

                if ( value )
                {
                    Stage.Debug = true;
                }
            }
        }

        public Actor Debug()
        {
            DebugFlag = true;

            return this;
        }

        public new string ToString()
        {
            return this._name;
        }
    }
}
