using System;
using RunKeeper8.Contracts.Exercise;

namespace RunKeeper8.Domain.Exercise
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
