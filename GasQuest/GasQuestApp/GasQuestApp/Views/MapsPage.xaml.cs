using GasQuestApp.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsPage : ContentPage
    {
        bool showing = false;
        GoogleApiServices services;
        GeolocationRequest request;
        Xamarin.Essentials.Location location;

        public MapsPage()
        {
            InitializeComponent();

            BindingContext = services = new GoogleApiServices();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            location = await Geolocation.GetLocationAsync(request);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(4.0)));
        }

        private async void Location_Clicked(object sender, EventArgs e)
        {
            request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }


            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(4.0)));
        }

        private async void GasButton_Clicked(object sender, EventArgs e)
        {
            if (showing)
            {
                customMap.Pins.Clear();
            }

            var places = await services.GetGooglePlaces(location.Latitude.ToString(), location.Longitude.ToString());

            showing = true;

            foreach (var place in places.results)
            {
                var id = place.place_id;
                var googlePlace = await services.GetPlaceById(id);
                var distance = Math.Round((float)location.CalculateDistance(place.geometry.location.lat, place.geometry.location.lng, DistanceUnits.Miles), 2);

                Pin pin = new Pin
                {
                    Label = place.name,
                    Address = distance.ToString() + " Miles to Station.",
                    Tag = place.place_id,
                    Type = PinType.Place,
                    Icon = BitmapDescriptorFactory.FromBundle("gas_station"),
                    Position = new Position(place.geometry.location.lat, place.geometry.location.lng)
                };

                customMap.Pins.Add(pin);
            }

            showing = false;
        }

        private async void customMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var choice = await DisplayActionSheet(e.Pin.Label, "Cancel", null, "Get Route", "Go to Website");
            var googlePlace = await services.GetPlaceById(e.Pin.Tag.ToString());

            switch (choice)
            {
                case "Get Route":
                    await Browser.OpenAsync(googlePlace.result.url, BrowserLaunchMode.SystemPreferred);
                    break;

                case "Go to Website":
                    if (googlePlace.result.website == null)
                    {
                        await DisplayAlert("Error", "This station has no website", "Okay");
                        break;
                    }
                    else
                    {
                        await Browser.OpenAsync(googlePlace.result.website, BrowserLaunchMode.SystemPreferred);
                        break;
                    }
                    
            }
        }
    }
}