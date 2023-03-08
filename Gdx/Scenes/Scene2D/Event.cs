namespace LibGDXSharp.Scenes.Scene2D
{
    /// <summary>
    /// The base class for all events.
    /// By default an event will "bubble" up through an actor's parent's handlers
    /// (see <see cref="Bubbles"/>).
    /// An actor's capture listeners can stop() an event to prevent child actors
    /// from seeing it.
    /// An Event may be marked as "handled" which will end its propagation outside
    /// of the Stage (see <see cref="IsHandled"/>). The default Actor.fire(Event)
    /// will mark events handled if an EventListener returns true.
    /// A cancelled event will be stopped and handled. Additionally, many actors
    /// will undo the side-effects of a canceled event. (See <see cref="IsCancelled"/>)
    /// </summary>
    /// <remarks>
    /// The original getters and setters for 'cancelled', 'handled', 'bubbles',
    /// 'stopped', 'stage', 'targetActor', 'listenerActor', and 'capture' have
    /// been removed and replaced with C# Properties.
    /// Therefore, Event.isHandled() is now Event.IsHandled etc.
    /// </remarks>
    public class Event : IPoolable
    {
        // The Stage for the Actor the event was fired on.
        public Stage? Stage { get; set; }

        // The Actor this event originated from.
        public Actor? TargetActor { get; set; }

        // The Actor this listener is attached to.
        public Actor? ListenerActor { get; set; }

        // true means event occurred during the capture phase
        public bool Capture { get; set; }

        // If true, after the event is fired on the target actor, it will also
        // be fired on each of the parent actors, all the way to the root.
        public bool Bubbles { get; set; } = true;

        // true means the event was handled (the stage will eat the input)
        public bool IsHandled { get; set; }

        // true means event propagation was stopped
        public bool IsStopped { get; set; }

        // true means propagation was stopped and any action that this event would cause should not happen
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Marks this event cancelled. This handles the event and stops
        /// the event propagation. It also cancels any default action that
        /// would have been taken by the code that fired the event.
        /// Eg, if the event is for a checkbox being checked, cancelling
        /// the event could uncheck the checkbox.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
            IsStopped   = true;
            IsHandled   = true;
        }

        public void Reset()
        {
            Stage         = null;
            TargetActor   = null;
            ListenerActor = null;
            Capture       = false;
            Bubbles       = true;
            IsHandled     = false;
            IsStopped     = false;
            IsCancelled   = false;
        }
    }
}
