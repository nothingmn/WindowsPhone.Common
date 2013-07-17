using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Storage
{
    /// <summary>
    /// Representative of a folder
    /// </summary>
    public interface IGroup
    {
        IGroup Parent { get; set; }
        ObservableCollection<IItem> Items { get; set; }
        ObservableCollection<IGroup> Groups { get; set; }
        string Name { get; set; }
        string FullName { get;  }
        string Guid { get; set; }
        string Title { get; set; }
        Uri RemoteUri { get; set; }
        Uri LocalUri { get; set; }

        Task<bool> Save();
        Task<bool> Load();
    }
}
