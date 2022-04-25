using GasQuestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GasQuestApp.Services
{
    public class GoogleApiServices
    {
        private JsonSerializer _serializer = new JsonSerializer();

        private static GoogleApiServices _ServiceClientInstance;

        public static GoogleApiServices ServiceClientInstance
        {
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new GoogleApiServices();
                return _ServiceClientInstance;
            }
        }
        private HttpClient client;
        public GoogleApiServices()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
        }

        public async Task<GooglePlaces> GetGooglePlaces(string lat, string lng)
        {
            GooglePlaces googlePlaces = new GooglePlaces();

            var response = await client.GetAsync($"api/place/textsearch/json?query=gasstation&location={lat}%2C{lng}&radius=700&type=gas_station&key=AIzaSyDzMDyEcen1vlhcA-wI4Ny4joZAq6Y8J2g&sensor=false");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    googlePlaces = await Task.Run(() =>
                       JsonConvert.DeserializeObject<GooglePlaces>(json)
                    ).ConfigureAwait(false);
                }
            }

            return googlePlaces;
        }

        public async Task<PlaceID> GetPlaceById(string id)
        {
            PlaceID placeId = new PlaceID();

            var response = await client.GetAsync($"api/place/details/json?place_id={id}&key=AIzaSyDzMDyEcen1vlhcA-wI4Ny4joZAq6Y8J2g");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    placeId = await Task.Run(() =>
                       JsonConvert.DeserializeObject<PlaceID>(json)
                    ).ConfigureAwait(false);
                }
            }

            return placeId;
        }
    }
}
