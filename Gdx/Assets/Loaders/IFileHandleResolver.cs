namespace LibGDXSharp.Assets.Loaders
{
    public interface IFileHandleResolver
    {
        public FileHandle? Resolve( string fileName );
    }
}
