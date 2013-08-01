using System.Collections.Generic;
using RunKeeper8.Contracts.Exercise;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Contracts.Serialization;

namespace RunKeeper8.Domain.Exercise
{
    public class History : IHistory
    {
        public IList<IHistoryItem> HistoryItems { get; set; }

        private readonly IRepository _repo;
        private ISerialize _serializer;
        public History(IRepository repo, ISerialize serializer)
        {
            _repo = repo;
            _serializer = serializer;
            Load();
        }

        public void Load()
        {
            HistoryItems = new List<IHistoryItem>();

        }

        public void Save()
        {
        }
    }
}
