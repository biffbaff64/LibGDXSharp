using StringBuilder = System.Text.StringBuilder;

namespace LibGDXSharp.Assets
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetDescriptor<T>
    {
        public string                      FileName   { get; set; }
        public Type                        Type       { get; set; }
        public AssetLoaderParameters< T >? Parameters { get; set; }
        public FileHandle?                 File       { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assetType"></param>
        /// <param name="parameters"></param>
        public AssetDescriptor( string fileName, Type assetType, AssetLoaderParameters< T >? parameters = null )
        {
            FileName   = fileName.Replace( '\\', '/' );
            Type       = assetType;
            Parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="assetType"></param>
        /// <param name="parameters"></param>
        public AssetDescriptor( FileHandle file, Type assetType, AssetLoaderParameters< T >? parameters = null )
        {
            FileName   = file.Path().Replace( '\\', '/' );
            File       = file;
            Type       = assetType;
            Parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            var sb = new StringBuilder();
            sb.Append( FileName );
            sb.Append( ", " );
            sb.Append( Type.FullName );

            return sb.ToString();
        }
    }
}
