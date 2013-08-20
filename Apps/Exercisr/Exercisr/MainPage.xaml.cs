using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using Exercisr.Contracts.ViewModels;
using Exercisr.Resources;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Communication;
using WindowsPhone.Contracts.Logging;

namespace Exercisr
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ILog _log = null;
        private ITrackingViewModel dataContext;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            dataContext = DI.Container.Current.Get<ITrackingViewModel>();
            this.DataContext = dataContext;

            //this stuff is a hack because it seems like the map has issues with databinding xaml to viewmodels for pololines
            //                <maps:Map.MapElements >
            //        <maps:MapPolyline StrokeColor="{Binding StrokeColor, Mode=TwoWay}" StrokeThickness="{Binding StrokeThickness, Mode=TwoWay}" Path="{Binding Coordinates, Mode=TwoWay}">
            //        </maps:MapPolyline>
            //    </maps:Map.MapElements>

            dataContext.UICoordinates.CollectionChanged += Coordinates_CollectionChanged;

            (dataContext as ViewModelBase).PropertyChanged += MainPage_PropertyChanged;
            (dataContext as ViewModelBase).Attach(this);

            line.StrokeColor = dataContext.StrokeColor;
            line.StrokeThickness = dataContext.StrokeThickness;
            Map.MapElements.Add(line);

            Loaded += MainPage_Loaded;
            _log = DI.Container.Current.Get<ILog>();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string type = "0";
            if (!NavigationContext.QueryString.TryGetValue("type", out type))
            {
                type = "1";
            }
            int id = 0;
            if (!int.TryParse(type, out id)) id = 1;

            dataContext.SetExerciseTypeId(id);
        }

        private void MainPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //_log.InfoFormat("Main Page Property Changed:{0}", e.PropertyName);
            if (e.PropertyName == "StrokeThickness") line.StrokeThickness = dataContext.StrokeThickness;
            if (e.PropertyName == "StrokeColor") line.StrokeColor = dataContext.StrokeColor;

            if (e.PropertyName == "StartVisibility")
            {
                if ((this.dataContext as ITrackingViewModel).StartVisibility == Visibility.Collapsed)
                {
                    line.Path.Clear();
                }
            }

        }

        private MapPolyline line = new MapPolyline();

        private void Coordinates_CollectionChanged(object sender,
                                                   System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var trigger in e.NewItems)
                {
                    line.Path.Add(trigger as GeoCoordinate);
                }
            }
        }

    }
}