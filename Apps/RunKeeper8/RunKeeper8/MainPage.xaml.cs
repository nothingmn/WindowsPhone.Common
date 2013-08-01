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

            dataContext = DI.Container.Current.Get<ITrackingViewModel>();
            this.DataContext = dataContext;

            //this stuff is a hack because it seems like the map has issues with databinding xaml to viewmodels for pololines
            //                <maps:Map.MapElements >
            //        <maps:MapPolyline StrokeColor="{Binding StrokeColor, Mode=TwoWay}" StrokeThickness="{Binding StrokeThickness, Mode=TwoWay}" Path="{Binding Coordinates, Mode=TwoWay}">
            //        </maps:MapPolyline>
            //    </maps:Map.MapElements>

            dataContext.Coordinates.CollectionChanged += Coordinates_CollectionChanged;

            (dataContext as ViewModelBase).PropertyChanged += MainPage_PropertyChanged;
            (dataContext as ViewModelBase).Attach(this);

            line.StrokeColor = dataContext.StrokeColor;
            line.StrokeThickness = dataContext.StrokeThickness;
            Map.MapElements.Add(line);
            
            Loaded += MainPage_Loaded;

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

        void MainPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StrokeThickness") line.StrokeThickness = dataContext.StrokeThickness;
            if (e.PropertyName == "StrokeColor") line.StrokeColor = dataContext.StrokeColor;

            if (e.PropertyName == "PublishSuccess")
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        var msg = "Successfully published to RunKeeper!";
                        if (!dataContext.PublishSuccess) msg = "Failed to publish to Runkeeper!";

                        MessageBox.Show(msg);
                    });
            }

        }
        MapPolyline line = new MapPolyline();
        void Coordinates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var trigger in e.NewItems)
                {
                    line.Path.Add(trigger as GeoCoordinate);
                }
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