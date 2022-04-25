using Firebase.Database;
using Firebase.Database.Query;
using GasQuestApp.Services;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GasQuestApp.ViewModels
{
    public class AddVehiclePageViewModel : INotifyPropertyChanged
    {
        public IAuth auth;

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        FirebaseClient firebaseClient = new FirebaseClient("https://gasquest-38b4c-default-rtdb.firebaseio.com/");

        public ICommand AddVehicleCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private string carName;

        public string CarName
        {
            get
            {
                return carName;
            }
            set
            {
                carName = value;
                OnPropertyChanged("CarName");
            }
        }

        public AddVehiclePageViewModel()
        {
            AddVehicleCommand = new Command(AddVehicle);
            auth = DependencyService.Get<IAuth>();
        }

        async void AddVehicle(object perameters)
        {
            await UpdateVehicle(CarName, "", "", "", "", "", "", "", "", "", "", "", "");

            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<Users> UpdateVehicle(string vehicle, string firstName, string lastName, string email, string uid, string totalSpent, string odometer, string mpg, string fillupCount, string home, string gallons, string pathToPhoto, string isPhoto)
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
            pathToPhoto = person.PathToPhoto;
            isPhoto = person.IsPhoto;
            
            

            if (user != null)
            {
                await App.Current.MainPage.DisplayAlert("Success", CarName + " added!", "Ok");

                var allPersons = await firebaseHelper.GetAllPersons();

                var toUpdateVehicle = (await firebaseClient
                  .Child("Users")
                  .OnceAsync<Users>())
                  .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                 .Child("Users")
                 .Child(toUpdateVehicle.Key)
                 .PutAsync(new Users() { Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons, PathToPhoto = pathToPhoto, IsPhoto = isPhoto, });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }
    }
}
