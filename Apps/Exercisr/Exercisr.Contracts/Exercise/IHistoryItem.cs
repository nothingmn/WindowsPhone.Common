using System;
using System.Collections.Generic;
using Exercisr.Contracts.Configuration;
using Exercisr.Contracts.Geo;
using Exercisr.Contracts.Services;
using Microsoft.Phone.Maps.Controls;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Contracts.Exercise
{

    public interface IHistoryItem
    {
        ISettings Settings { get; set; }
        string DisplayName { get; set; }
        DateTime StartTimestamp { get; set; }
        DateTime EndTimestamp { get; set; }
        int Id { get; set; }
        string ExerciseType { get; set; }
        double Distance { get; set; }
        double Calories { get; set; }
        TimeSpan Pace { get; set; }
        DateTime PublishDateTime { get; set; }
        bool PublishSuccess { get; set; }
        IList<ICoordinate> Coordinates { get; set; }
        string PublishBody { get; set; }
        void PublishToRunKeeper();
        string DistanceDisplay { get; }
        bool Save();

    }
}
