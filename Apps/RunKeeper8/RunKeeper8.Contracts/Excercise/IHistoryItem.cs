using System;

namespace RunKeeper8.Contracts.Excercise
{
    public interface IHistoryItem
    {
        string DisplayName { get; set; }
        DateTime Timestamp { get; set; }
        string Id { get; set; }
        string HistoryType { get; set; }
        object HistoryData { get; set; }
    }
}
