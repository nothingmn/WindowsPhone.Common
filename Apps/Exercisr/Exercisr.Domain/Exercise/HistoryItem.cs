using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Exercisr.Contracts.Configuration;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Geo;
using Exercisr.Contracts.Services;
using Exercisr.Domain.Geo;
using Microsoft.Phone.Maps.Controls;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Domain.Exercise
{

    public class HistoryItem : IHistoryItem
    {
        private readonly IRepository _repository;
        private readonly ILog _log;
        public ISettings Settings { get; set; }

        public HistoryItem(IRepository repository, ISettings settings, ILog log)
        {
            _repository = repository;
            _log = log;
            Settings = settings;
        }

        public string DisplayName { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int Id { get; set; }
        public int HistoryId { get; set; }
        public string ExerciseType { get; set; }
        public double Distance { get; set; }

        public string DistanceDisplay
        {
            get
            {
                string result = string.Format("{0:f2} mi", Distance * 0.6214);
                if (Settings.IsMetric)
                {
                    result = string.Format("{0:f2} km", Distance);
                }
                _log.InfoFormat("Distance Display:{0} ({1})", result, Id);
                return result;

            }

        }

        public double Calories { get; set; }
        public TimeSpan Pace { get; set; }
        public DateTime PublishDateTime { get; set; }
        public bool PublishSuccess { get; set; }
        public IList<ICoordinate> Coordinates { get; set; }
        public string PublishBody { get; set; }

        public bool Save()
        {
            HistoryDBItem db = new HistoryDBItem()
                {
                    DisplayName = DisplayName,
                    StartTimestamp = StartTimestamp,
                    EndTimestamp = EndTimestamp,
                    ExerciseType = ExerciseType,
                    Id = Id,
                    Distance = Distance,
                    Calories = Calories,
                    Pace = Pace,
                    PublishDateTime = PublishDateTime,
                    PublishSuccess = PublishSuccess
                };

            if (Id <= 0)
            {
                _repository.Insert<HistoryDBItem>(db).ContinueWith(t =>
                    {
                        this.Id = db.Id;
                        var coordArray = new List<Task<int>>();
                        foreach (var c in Coordinates)
                        {
                            var cc = new Coordinate();
                            cc.Altitude = c.Altitude;
                            cc.Course = c.Course;
                            cc.HorizontalAccuracy = c.HorizontalAccuracy;
                            cc.HistoryItemId = this.Id;
                            cc.Latitude = c.Latitude;
                            cc.Longitude = c.Longitude;
                            cc.Speed = c.Speed;
                            cc.VerticalAccuracy = c.VerticalAccuracy;                            
                            coordArray.Add(_repository.Insert<Coordinate>(cc));
                        }
                        Task.WaitAll(coordArray.ToArray());

                    }).Wait();
                return true;

            }
            else
            {
                _repository.Update<HistoryDBItem>(db, Id).ContinueWith(t =>
                    {
                        this.Id = db.Id;
                        var coordArray = new List<Task<int>>();
                        foreach (var c in Coordinates)
                        {
                            var cc = new Coordinate();
                            cc.Altitude = c.Altitude;
                            cc.Course = c.Course;
                            cc.HorizontalAccuracy = c.HorizontalAccuracy;
                            cc.HistoryItemId = this.Id;
                            cc.Latitude = c.Latitude;
                            cc.Longitude = c.Longitude;
                            cc.Speed = c.Speed;
                            cc.VerticalAccuracy = c.VerticalAccuracy;
                            coordArray.Add(_repository.Insert<Coordinate>(cc));
                        }
                        Task.WaitAll(coordArray.ToArray());
                    });
                return false;
            }
        }

        public void PublishToRunKeeper()
        {

            var activity = WindowsPhone.DI.Container.Current.Get<IActivity>();

            TimeSpan Time = new TimeSpan(EndTimestamp.Ticks - StartTimestamp.Ticks);
            activity.detect_pauses = true;
            activity.duration = Time.TotalSeconds;
            activity.total_calories = Calories;
            activity.total_distance = Distance/1000; //distance is in KM's and we need M's
            activity.equipment = "None";
            activity.type = ExerciseType;
            
            activity.start_time =
                StartTimestamp.ToUniversalTime().ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern);

            activity.notes = string.Format("I just {0} for ", activity.type.Replace("ing", "ed"));
            if (Time.TotalHours >= 1)
            {
                activity.notes = activity.notes +
                                 string.Format("{0} hours, ", Time.TotalHours.ToString("0"));
            }
            activity.notes = activity.notes +
                             string.Format(
                                 "{0} minutes for a distance of {2}, and burned {1} calories!",
                                 Time.TotalMinutes.ToString("0.0"),
                                 activity.total_calories.ToString("0.0"),
                                 activity.total_distance.ToString("0.0"));

            //move to settings
            activity.post_to_facebook = false;
            activity.post_to_twitter = false;

            activity.path = new List<IPath>();

            var counter = 0;
            foreach (var c in Coordinates)
            {
                var path = WindowsPhone.DI.Container.Current.Get<IPath>();
                path.altitude = c.Altitude;
                path.latitude = c.Latitude;
                path.longitude = c.Longitude;
                if (counter == 0)
                {
                    path.type = "Start";
                }
                else if (counter == Coordinates.Count - 1)
                {
                    path.type = "End";
                }
                else
                {
                    path.type = "gps";
                }

                activity.path.Add(path);
                counter++;
            }

            var publisher = WindowsPhone.DI.Container.Current.Get<IPublishActivity>();
            publisher.OnPublishComplete += publisher_OnPublishComplete;
            publisher.Publish(activity);

        }

        private void publisher_OnPublishComplete(IPublishActivity publishActivity, DateTime timestamp, bool success,
                                                 Exception e, string body)
        {
            PublishDateTime = timestamp;
            PublishSuccess = success;
            PublishBody = body;

        }

    }
}