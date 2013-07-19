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
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Resources;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Communication;

namespace RunKeeper8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ITrackingViewModel dataContext;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //var channel = DI.Container.Current.Get<IChannel>();
            var channel1 = DI.Container.Current.Get<IChannel>("byte");
            var channel2 = DI.Container.Current.Get<IChannel>("string");
            var channel3 = DI.Container.Current.Get<IChannel>("csv");

            dataContext = DI.Container.Current.Get<ITrackingViewModel>();
            this.DataContext = dataContext;

            //this stuff is a hack because it seems like the map has issues with databinding xaml to viewmodels for pololines
            //                <maps:Map.MapElements >
            //        <maps:MapPolyline StrokeColor="{Binding StrokeColor, Mode=TwoWay}" StrokeThickness="{Binding StrokeThickness, Mode=TwoWay}" Path="{Binding Coordinates, Mode=TwoWay}">
            //        </maps:MapPolyline>
            //    </maps:Map.MapElements>

            dataContext.Coordinates.CollectionChanged += Coordinates_CollectionChanged;

            (dataContext as ViewModelBase).PropertyChanged += MainPage_PropertyChanged;
            line.StrokeColor = dataContext.StrokeColor;
            line.StrokeThickness = dataContext.StrokeThickness;
            Map.MapElements.Add(line);
            
        }

        void MainPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StrokeThickness") line.StrokeThickness = dataContext.StrokeThickness;
            if (e.PropertyName == "StrokeColor") line.StrokeColor = dataContext.StrokeColor;
        }
        MapPolyline line = new MapPolyline();
        void Coordinates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var trigger in e.NewItems)
            {
                line.Path.Add(trigger as GeoCoordinate);
            }
        }

        private void PairMenuItem_Click(object sender, EventArgs e)
        {
            dataContext.PairCommand.Execute(null);
        }

        private void RunKeeperLoginMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/UI/OAuthWebViewPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}