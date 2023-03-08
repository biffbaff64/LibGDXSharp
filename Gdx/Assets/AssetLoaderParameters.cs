namespace LibGDXSharp.Assets
{
    public class AssetLoaderParameters<T>
    {
        /// <summary>
        /// Callback interface that will be invoked when the <see cref="AssetManager"/> loaded an asset.
        /// </summary>
        public interface ILoadedCallback
        {
            public void FinishedLoading( AssetManager assetManager, string fileName, Type type );
        }

        public ILoadedCallback? LoadedCallback { get; set; }
    }
}
