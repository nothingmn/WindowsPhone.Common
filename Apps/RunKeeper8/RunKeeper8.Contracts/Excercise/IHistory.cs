using System.Collections.Generic;

namespace RunKeeper8.Contracts.Excercise
{
    public interface IHistory
    {
        IList<IHistoryItem> HistoryItems { get; set; }
    }
}
