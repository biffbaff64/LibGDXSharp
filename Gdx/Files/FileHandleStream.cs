namespace LibGDXSharp.Files
{
    public class FileHandleStream : FileHandle
    {
        public FileHandleStream( string path )
            : base( path, IFile.FileType.Absolute )
        {
        }

        public bool IsDirectory()
        {
            return false;
        }

        public new long Length()
        {
            return 0;
        }

        public bool Exists()
        {
            return true;
        }

        public FileHandle Child( string name )
        {
            throw new NotSupportedException();
        }

        public FileHandle Sibling( string name )
        {
            throw new NotSupportedException();
        }

        public FileHandle Parent()
        {
            throw new NotSupportedException();
        }

        public new StreamReader Read()
        {
            throw new NotSupportedException();
        }

        public new StreamWriter Write( bool overwrite )
        {
            throw new NotSupportedException();
        }

        public FileHandle[] List()
        {
            throw new NotSupportedException();
        }

        public void Mkdirs()
        {
            throw new NotSupportedException();
        }

        public bool Delete()
        {
            throw new NotSupportedException();
        }

        public bool DeleteDirectory()
        {
            throw new NotSupportedException();
        }

        public void CopyTo( FileHandle dest )
        {
            throw new NotSupportedException();
        }

        public void MoveTo( FileHandle dest )
        {
            throw new NotSupportedException();
        }
    }
}
