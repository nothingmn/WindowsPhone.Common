using System;
using System.Threading.Tasks;
using Windows.Storage;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Storage;

namespace WindowsPhone.Storage
{
    public class Item : IItem
    {
        private DataStorageHelper _helper;

        private ILog _log;
        public Item(DataStorageHelper helper, ILog log)
        {
            _helper = helper;
            _log = log;
        }
        public byte[] Contents { get; set; }

        public string Name { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}{2}{1}", (this.Parent != null) ? Parent.FullName : "", this.Name,
                                     System.IO.Path.DirectorySeparatorChar);
            }
        }


        public IGroup Parent { get; set; }
        public string Title { get; set; }
        public string Guid { get; set; }
        public Uri RemoteUri { get; set; }
        public Uri LocalUri { get; set; }

        private IStorageFolder ResolveStorageFolderForSave()
        {
            IStorageFolder sf = null;
            if (this.Parent != null) this.Parent.Save().Wait();
            var parent = (this.Parent as Group);
            if (parent != null && parent.StorageFolder != null) sf = parent.StorageFolder;
            if (sf == null) sf = _helper.Root;
            return sf;
        }

        public async Task<bool> Save()
        {
            IStorageFolder StorageFolder = ResolveStorageFolderForSave();
            _log.InfoFormat("Save Item:Name:{0}, Path:{1}", this.Name, StorageFolder.Path);
            _helper.WriteFile(this.Name, StorageFolder, Contents).Wait();
            return true;

        }
        private IStorageFolder ResolveStorageFolderForLoad()
        {
            var p = Parent as Group;
            if (p != null) return p.StorageFolder;
            return _helper.Root;
        }
        public async Task<bool> Load()
        {
            IStorageFolder StorageFolder = ResolveStorageFolderForLoad();
            _log.InfoFormat("Read Item:Name:{0}, Path:{1}", this.Name, StorageFolder.Path);

            IStorageFile file = null;
            try
            {
                StorageFolder.GetFileAsync(this.Name).AsTask().ContinueWith(t =>
                {
                    if(t!=null && t.Result!=null) file = t.Result;
                }).Wait();
            }
            catch (Exception)
            {
                
            }
            if (file != null)
            {

                _helper.ReadFile(this.Name, StorageFolder).ContinueWith(task =>
                    {
                        this.Contents = task.Result;
                    }).Wait();
            }
            return true;
        }
    }
}
