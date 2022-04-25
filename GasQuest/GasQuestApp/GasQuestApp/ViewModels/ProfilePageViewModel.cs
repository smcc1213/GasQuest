using Firebase.Database;
using Firebase.Database.Query;
using GasQuestApp.Services;
using GasQuestApp.Views;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GasQuestApp.ViewModels
{
    class ProfilePageViewModel : INotifyPropertyChanged
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        IAuth auth;
        FirebaseClient firebaseClient = new FirebaseClient("https://gasquest-38b4c-default-rtdb.firebaseio.com/");
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ChangeCityCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ProfileTappedCommand { get; set; }
        public ICommand ResetDataCommand { get; set; }


        private string userCity;
        private string pathSource;
        private string userMPG;
        private string totalSpent;
        private string mileage;
        string isPhotoValue;
      
        public string UserCity { 
            get 
            { 
                return userCity; 
            } 
            set 
            { 
                userCity = value; 
                OnPropertyChanged("UserCity"); 
            } 
        }
        public string PathSource
        {
            get { return pathSource; }
            set 
            { 
                pathSource = value;
                OnPropertyChanged("PathSource");
            }
        }

        public string MPG
        {
            get { return userMPG; }
            set
            {
                userMPG = value;
                OnPropertyChanged("MPG");
            }
        }

        public string Mileage
        {
            get { return mileage; }
            set
            {
                mileage = value;
                OnPropertyChanged("Mileage");
            }
        }

        public string TotalSpent
        {
            get { return totalSpent; }
            set
            {
                totalSpent = value;
                OnPropertyChanged("TotalSpent");
            }
        }


        public ProfilePageViewModel()
        {
            auth = DependencyService.Get<IAuth>();
            ChangeCityCommand = new Command(ChangeCity);
            LogoutCommand = new Command(LogOut);
            ProfileTappedCommand = new Command(ProfileTapped);
            ResetDataCommand = new Command(ResetData);
         
        }

        async void ProfileTapped(object obj)
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });

            await LoadPhotoAsync(photo);

            await UpdatePhotoPath(PathSource, isPhotoValue, "", "", "", "", "", "", "", "", "", "", "");

            await LoadPhotoAsync(photo);
        }

        async void LogOut(object obj)
        {
            var choice = await App.Current.MainPage.DisplayAlert("Log Out of Gas Quest?", null, "Yes", "No");

            if (choice)
            {
                firebaseHelper.auth.SignOut();
                Application.Current.MainPage = new LoginPage();
            }
        }

        async void ChangeCity(object obj)
        {
            string result = await App.Current.MainPage.DisplayPromptAsync("Change Home City", "What's is the name of your home city?");

            if (!string.IsNullOrEmpty(result))
            {
                UserCity = result;
                await UpdateCity(UserCity, "", "", "", "", "", "", "", "", "", "", "", "");

            }
        }

        public async Task<Users> UpdateCity(string home, string firstName, string lastName, string email, string uid, string totalSpent, string odometer, string mpg, string fillupCount, string vehicle, string gallons, string pathToPhoto, string isPhoto)
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            uid = person.Uid;
            firstName = person.FirstName;
            lastName = person.LastName;
            email = person.Email;
            totalSpent = person.TotalSpent;
            odometer = person.OdometerReading;
            mpg = person.MPG;
            fillupCount = person.FillupCount;
            gallons = person.GallonsRecieved;
            vehicle = person.Vehicle;
            pathToPhoto = person.PathToPhoto;
            isPhoto = person.IsPhoto;


            if (user != null)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Home City changed!", "Ok");

                var allPersons = await firebaseHelper.GetAllPersons();

                var toUpdateCity = (await firebaseClient
                  .Child("Users")
                  .OnceAsync<Users>())
                  .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                 .Child("Users")
                 .Child(toUpdateCity.Key)
                 .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }

        public async Task<Users> UpdatePhotoPath(string pathToPhoto, string isPhoto, string home, string firstName, string lastName, string email, string uid, string totalSpent, string odometer, string mpg, string fillupCount, string vehicle, string gallons)
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            uid = person.Uid;
            firstName = person.FirstName;
            lastName = person.LastName;
            email = person.Email;
            totalSpent = person.TotalSpent;
            odometer = person.OdometerReading;
            mpg = person.MPG;
            fillupCount = person.FillupCount;
            home = person.HomeCity;
            gallons = person.GallonsRecieved;
            vehicle = person.Vehicle;
            isPhoto = person.IsPhoto;


            if (user != null)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Profile Picture changed!", "Ok");

                var allPersons = await firebaseHelper.GetAllPersons();
                isPhoto = "true";
                isPhotoValue = isPhoto;

                var toUpdateCity = (await firebaseClient
                  .Child("Users")
                  .OnceAsync<Users>())
                  .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                 .Child("Users")
                 .Child(toUpdateCity.Key)
                 .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }

        async Task LoadPhotoAsync(FileResult photo)
        {

            if (photo == null)
            {
                PathSource = null;
                return;
            }


            var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PathSource = newFile;

        }


        async void ResetData(object obj)
        {
            var answer = await App.Current.MainPage.DisplayActionSheet("Reset all vehicle data?", "Cancel", null,"Reset All Vehicle Data", "Reset MPG and Mileage");

            switch (answer)
            {
                case "Reset All Vehicle Data":
                    Mileage = "0";
                    TotalSpent = "0";
                    MPG = "0";
                    await ResetAll(Mileage, TotalSpent, Mileage, "", "", "", "", "", "", "", "", "", "");
                    break;
                case "Reset MPG and Mileage":
                    Mileage = "0";
                    MPG = "0";
                    await ResetMileageAndMPG(Mileage, MPG, "", "", "", "", "", "", "", "", "", "", "");
                    break;
            }
                   
        }


        public async Task<Users> ResetAll(string mpg, string totalSpent, string odometer, string fillupCount, string firstName, string lastName, string email, string uid, string vehicle, string home, string gallons, string pathToPhoto, string isPhoto)
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            uid = person.Uid;
            firstName = person.FirstName;
            lastName = person.LastName;
            email = person.Email;
            totalSpent = person.TotalSpent;
            odometer = person.OdometerReading;
            mpg = person.MPG;
            fillupCount = person.FillupCount;
            home = person.HomeCity;
            gallons = person.GallonsRecieved;
            vehicle = person.Vehicle;
            pathToPhoto = person.PathToPhoto;
            isPhoto = person.IsPhoto;


            if (user != null)
            {
                var allPersons = await firebaseHelper.GetAllPersons();

                mpg = MPG;
                odometer = Mileage;
                totalSpent = TotalSpent;
                fillupCount = "0";

                var toUpdateMPG = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<Users>())
                    .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                    .Child("Users")
                    .Child(toUpdateMPG.Key)
                    .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }

        public async Task<Users> ResetMileageAndMPG(string odometer, string mpg, string fillupCount, string totalSpent, string firstName, string lastName, string email, string uid, string vehicle, string home, string gallons, string pathToPhoto, string isPhoto)
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            uid = person.Uid;
            firstName = person.FirstName;
            lastName = person.LastName;
            email = person.Email;
            totalSpent = person.TotalSpent;
            odometer = person.OdometerReading;
            mpg = person.MPG;
            fillupCount = person.FillupCount;
            home = person.HomeCity;
            gallons = person.GallonsRecieved;
            vehicle = person.Vehicle;
            pathToPhoto = person.PathToPhoto;
            isPhoto = person.IsPhoto;


            if (user != null)
            {
                var allPersons = await firebaseHelper.GetAllPersons();

                mpg = MPG;
                odometer = Mileage;
                fillupCount = "0";

                var toUpdate = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<Users>())
                    .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                    .Child("Users")
                    .Child(toUpdate.Key)
                    .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
