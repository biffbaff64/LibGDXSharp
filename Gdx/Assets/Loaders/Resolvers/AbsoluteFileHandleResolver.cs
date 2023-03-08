namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class AbsoluteFileHandleResolver : IFileHandleResolver
    {
        public FileHandle? Resolve( string fileName )
        {
            return Gdx.Files?.Absolute( fileName );
        }
    }
}

