using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Windows.Storage;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Storage;

namespace WindowsPhone.Storage
{
    public class Group : IGroup
    {
        public IStorageFolder StorageFolder { get; set; }

        private DataStorageHelper _helper;
        private ILog _log;
        public Group(DataStorageHelper helper, ILog log)
        {
            Items = new ObservableCollection<IItem>();
            Groups = new ObservableCollection<IGroup>();

            Items.CollectionChanged += Files_CollectionChanged;
            Groups.CollectionChanged += Folders_CollectionChanged;

            _helper = helper;
            _log = log;

        }

        public ObservableCollection<IItem> Items { get; set; }
        public ObservableCollection<IGroup> Groups { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public Uri RemoteUri { get; set; }
        public Uri LocalUri { get; set; }


        public string FullName
        {
            get
            {
                if (this.Parent != null)
                {
                    return string.Format("{0}{1}{2}", this.Parent.FullName, System.IO.Path.DirectorySeparatorChar,
                                         this.Name);
                }
                else
                {
                    return string.Format("{0}", this.Name);
                }
            }
        }


        public IGroup Parent { get; set; }


        private void Folders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IGroup n in e.NewItems)
                {
                    n.Parent = this;
                }
            }
        }

        private void Files_CollectionChanged(object sender,
                                             System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IItem n in e.NewItems)
                {
                    n.Parent = this;
                }
            }
        }


        private IStorageFolder ResolveStorageFolderForSave()
        {
            IStorageFolder sf = this.StorageFolder;
            if (this.Parent != null) this.Parent.Save();
            var parent = (this.Parent as Group);
            if (sf == null)
            {
                if (parent != null && parent.StorageFolder != null) sf = parent.StorageFolder;

                if (sf == null) sf = _helper.Root;
            }
            return sf;
        }
        public async Task<bool> Save()
        {
            this.StorageFolder = ResolveStorageFolderForSave();
            _log.InfoFormat("Save Group, :Name:{0}, Resolved Path:{1}", this.Name, StorageFolder.Path);

            if (Parent == null)
            {

                _helper.CreateFolder(this.StorageFolder, this.Name).ContinueWith(task =>
                    {
                        this.StorageFolder = task.Result;
                    }).Wait();
            }
            else
            {
                //save me
                _helper.CreateFolder((this.Parent as Group).StorageFolder, this.Name).ContinueWith(task =>
                    {
                        this.StorageFolder = task.Result;
                    }).Wait();
            }
            return true;
        }
        private IStorageFolder ResolveStorageFolderForLoad()
        {
            IStorageFolder sf = this.StorageFolder;
            if (sf == null) sf = _helper.Root;
            return sf;
        }
        public async Task<bool> Load()
        {
            //we are at the root
            this.StorageFolder = ResolveStorageFolderForLoad();
            Task[] tasks = new Task[]
                {
                    StorageFolder.GetFilesAsync().AsTask().ContinueWith<Task<IReadOnlyList<StorageFile>>>(task =>
                        {
                            
                            var files = task.Result;
                            foreach (var f in files)
                            {
                                _log.InfoFormat("Load Group, Found File:Name:{0}, Path:{1}", f.Name, StorageFolder.Path);

                                Item i = new Item(_helper, this._log);
                                i.Name = f.Name;
                                this.Items.Add(i);
                            }
                            return task;
                        }),
                    StorageFolder.GetFoldersAsync().AsTask().ContinueWith(task =>
                        {
                            var folders = task.Result;
                            foreach (var f in folders)
                            {
                                _log.InfoFormat("Load Group, Found Folder:Name:{0}, Path:{1}", f.Name, StorageFolder.Path);

                                Group g = new Group(_helper, this._log);
                                g.StorageFolder = f;
                                g.Name = f.Name;
                                this.Groups.Add(g);
                                g.Load().Wait();
                            }
                        })

                };
            Task.WaitAll(tasks);
            return true;
        }
    }
}