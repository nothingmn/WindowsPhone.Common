using System.Collections.Generic;

namespace RunKeeper8.Contracts.Exercise
{
    public interface IHistory
    {
        IList<IHistoryItem> HistoryItems { get; set; }

        void Load();
        void Save();
    }
}
