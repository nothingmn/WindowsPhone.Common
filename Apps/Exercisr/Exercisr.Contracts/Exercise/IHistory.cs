using System.Collections.Generic;

namespace Exercisr.Contracts.Exercise
{
    public delegate void HistoryItemsChanged(IHistory history, IList<IHistoryItem> changedItems);

    public interface IHistory
    {
        IList<IHistoryItem> HistoryItems { get; set; }
        event HistoryItemsChanged OnHistoryItemsChanged;

        void Load();
        void Save();
    }
}
