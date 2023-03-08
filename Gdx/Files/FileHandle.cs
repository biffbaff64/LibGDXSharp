using System.Text;

namespace LibGDXSharp.Files
{
    /// <summary>
    /// 
    /// </summary>
    public class FileHandle
    {
        public FileInfo?       File { get; set; }
        public IFile.FileType? Type { get; set; }

        private readonly string _targetFilename = string.Empty;

        /// <summary>
        /// Represents a file or directory on the filesystem, classpath, app storage
        /// or assets directory. FileHandles are created via a Files instance.
        /// Because some of the file types are backed by composite files and may be
        /// compressed (for example, if they are found via the classpath), the methods
        /// for extracting a path() or file() may not be appropriate for all types. Use
        /// the Reader or Stream methods here to hide these dependencies from your
        /// platform independent code.
        /// </summary>
        protected FileHandle()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public FileHandle( string fileName )
        {
            this.File = new FileInfo( fileName );
            this.Type = IFile.FileType.Absolute;

            _targetFilename = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public FileHandle( FileInfo file )
        {
            this.File = file;
            this.Type = IFile.FileType.Absolute;

            _targetFilename = file.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        public FileHandle( string fileName, IFile.FileType type )
        {
            this.File = new FileInfo( fileName );
            this.Type = type;

            _targetFilename = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Path()
        {
            if ( this.File != null )
            {
                return File.FullName;
            }

            throw new GdxRuntimeException( "FileHandle:Path - file does not exist: " + _targetFilename );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Name()
        {
            if ( this.File != null )
            {
                return File.Name;
            }

            throw new GdxRuntimeException( "FileHandle:Name - file does not exist: " + _targetFilename );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Extension()
        {
            if ( this.File != null )
            {
                return File.Extension;
            }

            throw new GdxRuntimeException( "FileHandle:Extension - file does not exist: " + _targetFilename );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string NameWithoutExtension()
        {
            if ( this.File != null )
            {
                var fileName = this.File.Name;
                var dotIndex = fileName.LastIndexOf( ".", StringComparison.Ordinal );

                return dotIndex == -1 ? fileName : fileName.Substring( 0, dotIndex );
            }

            throw new GdxRuntimeException( "FileHandle:NameWithoutExtension - file does not exist: " + _targetFilename );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PathWithoutExtension()
        {
            if ( this.File != null )
            {
                var filePath = this.File.FullName;
                var dotIndex = filePath.LastIndexOf( ".", StringComparison.Ordinal );

                return dotIndex == -1 ? filePath : filePath.Substring( 0, dotIndex );
            }

            throw new GdxRuntimeException( "FileHandle.PathWithoutExtension - file does not exist: " + _targetFilename );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public FileStream Read()
        {
            if ( File == null )
            {
                throw new GdxRuntimeException( "FileHandle.Read - file not found: " + _targetFilename );
            }

            if ( this.Type == IFile.FileType.Classpath
                 || ( this.Type == IFile.FileType.Internal && !this.File.Exists )
                 || ( this.Type == IFile.FileType.Local && !this.File.Exists ) )
            {
                try
                {
                    var input = File.OpenRead();

                    return input;
                }
                catch ( Exception ex )
                {
                    throw new GdxRuntimeException( "File not found: " + File.Name + " (" + Type + ")", ex );
                }
            }

            return File.OpenRead();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public BufferedStream Read( int bufferSize )
        {
            if ( File == null )
            {
                throw new GdxRuntimeException( "FileHandle.Read - file not found: " + _targetFilename );
            }

            return new BufferedStream( File.OpenRead(), bufferSize );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StreamReader Reader()
        {
            return new StreamReader( Read() );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public StreamReader Reader( Encoding charset )
        {
            var stream = Read();

            try
            {
                return new StreamReader( stream, charset );
            }
            catch ( Exception ex )
            {
                stream.Close();

                throw new GdxRuntimeException( "Error reading file: " + this, ex );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public BufferedStream Reader( int bufferSize )
        {
            return Read( bufferSize );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public BufferedStream Reader( int bufferSize, Encoding charset )
        {
            return Reader( bufferSize );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public string ReadString( Encoding? charset = null )
        {
            var           output = new StringBuilder( EstimateLength() );
            StreamReader? reader = null;

            try
            {
                reader = charset == null ? new StreamReader( Read() ) : new StreamReader( Read(), charset );

                output.Append( reader.ReadToEnd() );
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error reading layout file - " + this, ex );
            }
            finally
            {
                reader?.Close();
            }

            return output.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public byte[] ReadBytes()
        {
            var input = Read();

            try
            {
                var buffer = new byte[ 16 * 1024 ];

                using var ms = new MemoryStream();

                int read;

                while ( ( read = input.Read( buffer, 0, buffer.Length ) ) > 0 )
                {
                    ms.Write( buffer, 0, read );
                }

                return ms.ToArray();
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error reading file: " + this, ex );
            }
            finally
            {
                input.Close();
            }
        }

        private int EstimateLength()
        {
            var length = ( int )Length();

            return length != 0 ? length : 512;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long Length()
        {
            if ( File != null
                 && ( Type == IFile.FileType.Classpath || ( Type == IFile.FileType.Internal && !File.Exists ) ) )
            {
                var input  = Read();
                var length = input.Length;

                input.Close();

                return length;
            }

            return File!.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public int ReadBytes( byte[] bytes, int offset, int size )
        {
            var input    = Read();
            var position = 0;

            try
            {
                while ( true )
                {
                    var count = input.Read( bytes, offset + position, size - position );

                    if ( count <= 0 )
                    {
                        break;
                    }

                    position += count;
                }
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error Reading File: " + this, ex );
            }
            finally
            {
                input.Close();
            }

            return position - offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="append"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public FileStream Write( bool append )
        {
            if ( Type == IFile.FileType.Classpath )
            {
                throw new GdxRuntimeException( "Cannot write to a classpath file: " + File, null );
            }

            if ( Type == IFile.FileType.Internal )
            {
                throw new GdxRuntimeException( "Cannot write to an internal file: " + File, null );
            }

            try
            {
                return System.IO.File.OpenWrite( File.FullName );
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error Writing File: " + File + " (" + Type + ")", ex );
            }
        }

        public BufferedStream Write( bool append, int bufferSize )
        {
            var stream = Write( append );

            return new BufferedStream( stream, bufferSize );
        }

        public void Write( FileStream input, bool append )
        {
            FileStream output = null;

            try
            {
                output = Write( append );

                output.CopyTo( input );
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error stream writing to file: " + File + " (" + Type + ")", ex );
            }
            finally
            {
                output?.Close();
                input.Close();
            }
        }
    }
}
