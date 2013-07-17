using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using WindowsPhone.Contracts.Logging;

namespace WindowsPhone.Storage
{

    public class DataStorageHelper
    {
        private ILog _log;
        private IStorageFolder _Root = null;
        public IStorageFolder Root
        {
            get { return _Root; }
            set { _Root = value; }
        }

        private IStorageFolder _store;

        public DataStorageHelper(ILog log, IStorageFolder storageFolder = null)
        {
            this._log = log;
            _store = storageFolder;
            _Root = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (_store == null) _store = _Root;
        }

        public async Task<IStorageFolder> CreateFolder(IStorageFolder parent, string name)
        {
            return await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
        }

        public async Task<byte[]> ReadFile(string name, IStorageFolder folder)
        {            
            // Get the file.
            var file = await folder.OpenStreamForReadAsync(name);
            byte[] buffer = new byte[file.Length];
            //read it
            file.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public async Task<bool> WriteFile(string name, IStorageFolder folder, byte[] contents = null)
        {
            // Get the file.
            var file = await folder.OpenStreamForWriteAsync(name, CreationCollisionOption.ReplaceExisting);

            // write the data
            if (contents != null)
            {
                file.Write(contents, 0, contents.Length);
            }
            file.Dispose();
            return true;
        }


    }
}