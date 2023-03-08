namespace LibGDXSharp.Scenes.Scene2D
{
    public abstract class Action : IPoolable
    {
        /// <summary>
        /// 
        /// </summary>
        public Pool< object >? Pool { get; set; }

        /// <summary>
        /// The actor this action targets, or null if a target has not been set.
        /// </summary>
        public Actor? Target { get; set; }

        /// <summary>
        /// The actor this action is attached to, or null if it is not attached.
        /// </summary>
        private Actor? _actor;

        /// <summary>
        /// Resets the optional state of this action to as if it were
        /// newly created, allowing the action to be pooled and reused.
        /// State required to be set for every usage of this action or
        /// computed during the action does not need to be reset.
        ///
        /// The default implementation calls <see cref="Restart"/>
        ///
        /// If a subclass has optional state, it must override this method,
        /// call super, and reset the optional state.
        /// </summary>
        public void Reset()
        {
            _actor = null;
            Target = null;
            Pool   = null;

            Restart();
        }

        /// <summary>
        /// Updates the action based on time. Typically this is
        /// called each frame by <see cref="Actor"/>.
        /// </summary>
        /// <param name="delta">Time in seconds since the last frame.</param>
        /// <returns>
        /// true if the action is done. This method may continue
        /// to be called after the action is done.
        /// </returns>
        public abstract bool Act( float delta );

        /// <summary>
        /// Sets the state of the action so it can be run again.
        /// </summary>
        public virtual void Restart()
        {
        }

        /// <summary>
        /// Sets the actor this action is attached to. This also sets the
        /// target actor if it is null. This method is called automatically
        /// when an action is added to an actor. This method is also called
        /// with null when an action is removed from an actor.
        /// When set to null, if the action has a pool then the action is
        /// returned to the pool (which calls reset()) and the pool is set
        /// to null. If the action does not have a pool, reset() is not called.
        /// This method is not typically a good place for an action subclass
        /// to query the actor's state because the action may not be executed
        /// for some time, eg it may be delayed. The actor's state is best
        /// queried in the first call to act(float).
        /// For a TemporalAction, use TemporalAction#begin().
        /// </summary>
        /// <param name="actor"></param>
        public void SetActor( Actor? actor )
        {
            this._actor = actor;

            if ( Target == null ) Target = actor;

            if ( actor == null )
            {
                if ( Pool != null )
                {
                    Pool.Free( this );
                    Pool = null;
                }
            }
        }

        /// <summary>
        /// Returns null if the action is not attached to an actor.
        /// </summary>
        public Actor? GetActor()
        {
            return _actor;
        }

        public override string ToString()
        {
            var name     = GetType().Name;
            var dotIndex = name.LastIndexOf( '.' );

            if ( dotIndex != -1 )
            {
                name = name.Substring( dotIndex + 1 );
            }

            if ( name.EndsWith( "Action" ) )
            {
                // NB: Rider suggested using 'range indexer' here. I opted
                // against it because, to me, name[ ..^6 ] looks like gibberish...
                name = name.Substring( 0, name.Length - 6 );
            }

            return name;
        }
    }
}
