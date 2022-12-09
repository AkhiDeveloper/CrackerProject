namespace CrackerProject.API.Model
{
    public class StorageDirectory
    {
        private string _name;
        private StorageDirectory? _parentDirectory;

        public StorageDirectory(string directory_name, StorageDirectory? parent_directory = null)
        {
            _name = directory_name;
            _parentDirectory = parent_directory;
        }

        public StorageDirectory(string? path=null)
        {
            if(path == null)
            {
                _name = "folder_" + DateTime.UtcNow.ToString()  ;
                _parentDirectory = null;
                return;
            }
            else
            {
                _name = Path.GetDirectoryName(path);
                if (!Path.IsPathRooted(path))
                {
                    _parentDirectory = null;
                }
                _parentDirectory = new StorageDirectory(Path.GetPathRoot(path));
            }
            
        }

        public string Name { get { return _name; } }
        public void ChangeName(string newname)
        {
            _name = newname;
        }
        public StorageDirectory? GetParentDirectory()
        {
            return _parentDirectory;
        }
        public StorageDirectory Child(string directory_name)
        {
            return new StorageDirectory(directory_name, _parentDirectory);
        }

        public string GetPath(string? filename = null)
        {
            string path;
            if(_parentDirectory == null)
            {
                path = Path.Combine(_name);
            }
            else
            {
                path = Path.Combine(_parentDirectory.GetPath(),_name);
            }
            if(filename != null)
            {
                path = Path.Combine(path, filename);
            }
            return path;
        }
    }
}
