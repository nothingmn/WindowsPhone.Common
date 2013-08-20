using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Geo;
using Exercisr.Domain.Geo;
using Microsoft.Phone.Maps.Controls;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts.DI;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Contracts.Serialization;

namespace Exercisr.Domain.Exercise
{
    public class History : ViewModelBase, IHistory, INotifyPropertyChanged
    {

        public event HistoryItemsChanged OnHistoryItemsChanged;

        private readonly IRepository _repository;
        private readonly IDIContainer _container;
        private IList<IHistoryItem> _historyItems = new List<IHistoryItem>();

        public IList<IHistoryItem> HistoryItems
        {
            get { return _historyItems; }
            set
            {
                _historyItems = value;
                OnPropertyChanged("HistoryItems");
            }
        }

        public History(IRepository repository, IDIContainer container)
        {
            _repository = repository;
            _container = container;
        }


        public void Save()
        {
            IList<IHistoryItem> changedItems = new List<IHistoryItem>();
            foreach (var historyItem in HistoryItems)
            {
                if (historyItem.Save())
                {
                    changedItems.Add(historyItem);
                }
            }
            if (OnHistoryItemsChanged != null) OnHistoryItemsChanged(this, changedItems);
        }



        public void Load()
        {
            _repository.Query<HistoryDBItem>("select * from HistoryDBItem").ContinueWith(t =>
                {
                    IList<IHistoryItem> items = new List<IHistoryItem>();

                    foreach (var r in t.Result)
                    {
                        var newItem = _container.Get<IHistoryItem>();

                        newItem.Calories = r.Calories;
                        newItem.DisplayName = r.DisplayName;
                        newItem.Distance = r.Distance;
                        newItem.ExerciseType = r.ExerciseType;
                        newItem.Coordinates = new List<ICoordinate>();
                        newItem.EndTimestamp = r.EndTimestamp;
                        newItem.Id = r.Id;
                        newItem.Pace = r.Pace;
                        newItem.PublishDateTime = r.PublishDateTime;
                        newItem.PublishSuccess = r.PublishSuccess;
                        newItem.StartTimestamp = r.StartTimestamp;


                        _repository.Query<Coordinate>("select * from Coordinate where HistoryItemId=?",
                                                      newItem.Id).ContinueWith(hctask =>
                                                          {
                                                              newItem.Coordinates =
                                                                  (from c in hctask.Result select c).ToArray();
                                                          }).Wait();
                        items.Add(newItem);

                    }
                    _historyItems = new List<IHistoryItem>(from i in items select i); //.ToArray();
                    if (OnHistoryItemsChanged != null) OnHistoryItemsChanged(this, this.HistoryItems);

                });
        }
   }
}