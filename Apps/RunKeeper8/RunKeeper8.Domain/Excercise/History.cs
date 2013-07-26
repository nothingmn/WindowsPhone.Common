using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Excercise;

namespace RunKeeper8.Domain.Excercise
{
    public class History : IHistory
    {
        public IList<IHistoryItem> HistoryItems { get; set; }
    }
}
