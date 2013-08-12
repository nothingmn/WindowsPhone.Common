using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Geo;
using Exercisr.Domain.Geo;
using Microsoft.Phone.Maps.Controls;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Contracts.Serialization;

namespace Exercisr.Domain.Exercise
{
    public class History : IHistory
    {

        public event HistoryItemsChanged OnHistoryItemsChanged;

        private readonly IRepository _repository;
        public IList<IHistoryItem> HistoryItems { get; set; }

        public History(IRepository repository)
        {
            _repository = repository;
        }


        public void Save()
        {
            IList<IHistoryItem> changedItems = new List<IHistoryItem>();
            foreach (var historyItem in HistoryItems)
            {
                if (historyItem.Id <= 0)
                {
                    changedItems.Add(historyItem);
                    HistoryDBItem db = new HistoryDBItem()
                        {
                            DisplayName = historyItem.DisplayName,
                            StartTimestamp = historyItem.StartTimestamp,
                            EndTimestamp = historyItem.EndTimestamp,
                            ExerciseType = historyItem.ExerciseType,
                            Id = historyItem.Id,
                            Distance = historyItem.Distance,
                            Calories = historyItem.Calories,
                            Pace = historyItem.Pace,
                            PublishDateTime = historyItem.PublishDateTime,
                            PublishSuccess = historyItem.PublishSuccess
                        };

                    _repository.Insert<HistoryDBItem>(db).ContinueWith(t =>
                        {
                            foreach (var c in historyItem.Coordinates)
                            {
                                var cc = new Coordinate();
                                cc.Altitude = c.Altitude;
                                cc.Course = c.Course;
                                cc.HorizontalAccuracy = c.HorizontalAccuracy;
                                cc.HistoryItemId = t.Result;
                                cc.Latitude = c.Latitude;
                                cc.Longitude = c.Longitude;
                                cc.Speed = c.Speed;
                                cc.VerticalAccuracy = c.VerticalAccuracy;
                                _repository.Insert<Coordinate>(cc).Wait();
                                var x = cc;
                            }
                        }).Wait();
                    if (OnHistoryItemsChanged != null) OnHistoryItemsChanged(this, changedItems);
                }
            }
            
        }

        public void Load()
        {
            _repository.Query<HistoryDBItem>("select * from HistoryDBItem").ContinueWith(t =>
                {
                    IList<HistoryItem> items = new List<HistoryItem>();

                    foreach (var r in t.Result)
                    {
                        var newItem = new HistoryItem()
                            {
                                Calories = r.Calories,
                                DisplayName = r.DisplayName,
                                Distance = r.Distance,
                                ExerciseType = r.ExerciseType,
                                Coordinates = new List<ICoordinate>(),
                                EndTimestamp = r.EndTimestamp,
                                Id = r.Id,
                                Pace = r.Pace,
                                PublishDateTime = r.PublishDateTime,
                                PublishSuccess = r.PublishSuccess,
                                StartTimestamp = r.StartTimestamp
                            };

                        _repository.Query<Coordinate>("select * from Coordinate where HistoryItemId=?",
                                                      newItem.Id).ContinueWith(hctask =>
                                                          {
                                                              newItem.Coordinates =
                                                                  (from c in hctask.Result select c).ToArray();
                                                          }).Wait();
                        items.Add(newItem);

                    }
                    this.HistoryItems = new List<IHistoryItem>(from i in items select i); //.ToArray();
                    if (OnHistoryItemsChanged != null) OnHistoryItemsChanged(this, this.HistoryItems);

                });
        }

        private class HistoryDBItem
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
}
