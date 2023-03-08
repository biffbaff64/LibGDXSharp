namespace LibGDXSharp.Assets
{
    public interface IAssetErrorListener
    {
        public void Error<T>( AssetDescriptor<T> asset, Exception throwable );
    }
}
