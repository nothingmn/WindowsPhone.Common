using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Threading.Tasks;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Geo;
using Microsoft.Phone.Maps.Controls;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Domain.Exercise
{

    public class HistoryItem : IHistoryItem
    {
        public string DisplayName { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int Id { get; set; }
        public int HistoryId { get; set; }
        public string ExerciseType { get; set; }
        public double Distance { get; set; }
        public double Calories { get; set; }
        public TimeSpan Pace { get; set; }
        public DateTime PublishDateTime { get; set; }
        public bool PublishSuccess { get; set; }
        public IList<ICoordinate> Coordinates { get; set; }


    }
}