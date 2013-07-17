using System.Collections.Generic;

namespace WindowsPhone.Contracts.Repository
{
    public class Page<T> : IPage<T>
    {
        public long CurrentPage { get; set; }

        public long ItemsPerPage { get; set; }

        public long TotalPages { get; set; }

        public long TotalItems { get; set; }

        public List<T> Items { get; set; }
    }
}
