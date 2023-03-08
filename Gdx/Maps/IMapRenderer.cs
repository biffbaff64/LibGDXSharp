namespace LibGDXSharp.Maps
{
    public interface IMapRenderer
    {
//        /// <summary>
//        /// Sets the projection matrix and viewbounds from the given camera.
//        /// If the camera changes, you have to call this method again.
//        /// The viewbounds are taken from the camera's position and viewport size as
//        /// well as the scale. This method will only work if the camera's direction
//        /// vector is (0,0,-1) and its up vector is (0, 1, 0), which are the defaults.
//        /// <param name="camera">The <see cref="OrthographicCamera"/> to use.</param>
//        /// </summary>
//        public void SetView( OrthographicCamera camera );
//
//        /// <summary>
//        /// Sets the projection matrix for rendering, as well as the bounds of
//        /// the map which should be rendered. Make sure that the frustum spanned
//        /// by the projection matrix coincides with the viewbounds.
//        /// </summary>
//        /// <param name="projectionMatrix"></param>
//        /// <param name="viewboundsX"></param>
//        /// <param name="viewboundsY"></param>
//        /// <param name="viewboundsWidth"></param>
//        /// <param name="viewboundsHeight"></param>
//        public void SetView( Matrix4 projectionMatrix,
//                             float viewboundsX,
//                             float viewboundsY,
//                             float viewboundsWidth,
//                             float viewboundsHeight );

        /// <summary>
        /// Renders all the layers of a map.
        /// </summary>
        public void Render();

        /// <summary>
        /// Renders the given layer indexes of a map.
        /// </summary>
        /// <param name="layers">The layers to render.</param>
        public void Render( int[] layers );
    }
}
