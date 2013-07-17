using System;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Storage
{
    /// <summary>
    /// representative of a file in a folder
    /// </summary>
    public interface IItem
    {
        IGroup Parent { get; set; }
        string Name { get; set; }
        string FullName { get;  }
        string Guid { get; set; }
        string Title { get; set; }
        Uri RemoteUri { get; set; }
        Uri LocalUri { get; set; }
        byte[] Contents { get; set; }


        Task<bool> Save();
        Task<bool> Load();
    }
}
