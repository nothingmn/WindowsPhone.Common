using System.Collections.Generic;

namespace WindowsPhone.Contracts.Repository
{
    public interface IPage<T>
    {
        long CurrentPage { get; set; }
        long ItemsPerPage { get; set; }
        long TotalPages { get; set; }
        long TotalItems { get; set; }
        List<T> Items { get; set; }
    }
}
