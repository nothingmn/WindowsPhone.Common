using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Excercise;

namespace RunKeeper8.Domain.Excercise
{
    public class HistoryItem : IHistoryItem     
    {
        public HistoryItem()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public string DisplayName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
        public string HistoryType { get; set; }
        public object HistoryData { get; set; }
    }
}
