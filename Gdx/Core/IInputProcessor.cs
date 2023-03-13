namespace LibGDXSharp.Core
{
    public interface IInputProcessor
    {
        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="keycode">One of the constants in <see cref="IInput.Keys"/></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool KeyDown( int keycode );

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        /// <param name="keycode">One of the constants in <see cref="IInput.Keys"/></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool KeyUp( int keycode );

        /// <summary>
        /// Called when a key was typed
        /// </summary>
        /// <param name="character"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool KeyTyped( char character );

        /// <summary>
        /// Called when the screen was touched or a mouse button was pressed.
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="pointer"></param>
        /// <param name="button"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool TouchDown( int screenX, int screenY, int pointer, int button );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="pointer"></param>
        /// <param name="button"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool TouchUp( int screenX, int screenY, int pointer, int button );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="pointer"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool TouchDragged( int screenX, int screenY, int pointer );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool MouseMoved( int screenX, int screenY );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amountX"></param>
        /// <param name="amountY"></param>
        /// <returns>TRUE if the input was processed.</returns>
        public bool Scrolled( float amountX, float amountY );
    }
}