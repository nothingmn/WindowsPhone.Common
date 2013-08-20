using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercisr.Domain.Exercise
{
    public class HistoryDBItem
    {
        public string DisplayName { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int Id { get; set; }
        public string ExerciseType { get; set; }
        public double Distance { get; set; }
        public double Calories { get; set; }
        public TimeSpan Pace { get; set; }
        public DateTime PublishDateTime { get; set; }
        public bool PublishSuccess { get; set; }
    }
}
