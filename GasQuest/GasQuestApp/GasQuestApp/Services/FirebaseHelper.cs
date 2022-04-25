using Firebase.Database;
using GasQuestApp.Services;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace GasQuestApp.Services
{
    public class FirebaseHelper
    {
        public IAuth auth;

        FirebaseClient firebase = new FirebaseClient("https://gasquest-38b4c-default-rtdb.firebaseio.com/");

        public FirebaseHelper()
        {
            auth = DependencyService.Get<IAuth>();
        }

        public async Task<List<Users>> GetAllPersons()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<Users>()).Select(item => new Users
              {
                  Uid = item.Object.Uid,
                  FirstName = item.Object.FirstName,
                  LastName = item.Object.LastName,
                  Email = item.Object.Email,
                  Vehicle = item.Object.Vehicle,
                  TotalSpent = item.Object.TotalSpent,
                  MPG = item.Object.MPG,
                  FillupCount = item.Object.FillupCount,
                  OdometerReading = item.Object.OdometerReading,
                  HomeCity = item.Object.HomeCity,
                  GallonsRecieved = item.Object.GallonsRecieved,
                  PathToPhoto = item.Object.PathToPhoto,
                  IsPhoto = item.Object.IsPhoto

              }).ToList();
        }

        public async Task<Users> GetPerson()
        {
            
            var allPersons = await GetAllPersons();
            string uid = auth.GetCurrentUser();

            await firebase
              .Child("Users")
              .OnceAsync<Users>();
            return allPersons.Where(a => a.Uid == uid).FirstOrDefault();
        }
    }
}
