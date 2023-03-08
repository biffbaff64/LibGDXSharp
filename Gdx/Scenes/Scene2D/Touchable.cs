namespace LibGDXSharp.Scenes.Scene2D
{
    public enum Touchable
    {
        /// All touch input events will be received by the actor and any children.
        Enabled,
        /// No touch input events will be received by the actor or any children.
        Disabled,
        /// No touch input events will be received by the actor, but children will still
        /// receive events. Note that events on the children will still bubble to the parent.
        ChildrenOnly
    }
}
