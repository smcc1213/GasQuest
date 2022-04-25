using Firebase.Database;
using Firebase.Database.Query;
using GasQuestApp.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace GasQuestApp.ViewModels
{
    class AddFillupPageViewModel : INotifyPropertyChanged
    {
        public IAuth auth;
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        FirebaseClient firebaseClient = new FirebaseClient("https://gasquest-38b4c-default-rtdb.firebaseio.com/");

        public ICommand AddFillupCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string gallonsReceived;
        private string amountSpent;
        private string odometerReading;

        string newMileageDifferenceValue;
        string newGallonsValue;
        string newMPGValue;
        string moneySpentString;


        public string GallonsReceived
        {
            get { return gallonsReceived; }
            set { gallonsReceived = value; OnPropertyChanged("GallonsRecieved"); }
        }

        public string AmountSpent
        {
            get { return amountSpent; }
            set { amountSpent = value; OnPropertyChanged("AmountSpent"); }
        }

        public string OdometerReading
        {
            get { return odometerReading; }
            set { odometerReading = value; OnPropertyChanged("OdometerReading"); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddFillupPageViewModel()
        {
            AddFillupCommand = new Command(AddFillup);
            auth = DependencyService.Get<IAuth>();
        }

        async void AddFillup(object parameters)
        {
            if (string.IsNullOrEmpty(GallonsReceived) || string.IsNullOrEmpty(AmountSpent) || string.IsNullOrEmpty(OdometerReading))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please have a value for every field", "Okay");
            }
            else
            { 
                await UpdateMPG(newMPGValue, "", "", "", "", "", "", "", "", "", "", "", "");
                await UpdateTotalSpent(AmountSpent, "", "", "", "", "", "", "", "", "", "", "", "");
                await UpdateOdometerReading(OdometerReading, "", "", "", "", "", "", "", "", "", "", "", "");

                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        public async Task<Users> UpdateMPG(string mpg, string firstName, string lastName, string email, string uid, string totalSpent, string vehicle, string odometer, string fillupCount, string home, string gallons, string pathToPhoto, string isPhoto)
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

                await GetTotalGallons();
                await CalculateMPG();

                gallons = fillupCount == "0" ? "0" : newGallonsValue;
                mpg = fillupCount == "0" ? "0" : newMPGValue;

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

        public async Task<Users> UpdateTotalSpent(string totalSpent, string firstName, string lastName, string email, string uid, string mpg, string vehicle, string odometer, string fillupCount, string home, string gallons, string pathToPhoto, string isPhoto)
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

                await CalculateTotalMoneySpent();
                totalSpent = moneySpentString;

                var toUpdateTotalSpent = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<Users>())
                    .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                    .Child("Users")
                    .Child(toUpdateTotalSpent.Key)
                    .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }


        public async Task<Users> UpdateOdometerReading( string odometer, string firstName, string lastName, string email, string uid, string totalSpent, string vehicle, string mpg, string fillupCount, string home, string gallons, string pathToPhoto, string isPhoto)
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
                await App.Current.MainPage.DisplayAlert("Success!", "New Fillup Added", "Ok");
                var allPersons = await firebaseHelper.GetAllPersons();

                odometer = NewOdometerReadingConversion(OdometerReading.ToString());
                fillupCount = GetNewFillUpCount(fillupCount);

                var toUpdateOdometerReading = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<Users>())
                    .Where(a => a.Object.Uid == user.ToString()).FirstOrDefault();

                await firebaseClient
                    .Child("Users")
                    .Child(toUpdateOdometerReading.Key)
                    .PutAsync(new Users() { PathToPhoto = pathToPhoto, IsPhoto = isPhoto, Vehicle = vehicle, Email = email, Uid = uid, FirstName = firstName, LastName = lastName, TotalSpent = totalSpent, OdometerReading = odometer, MPG = mpg, FillupCount = fillupCount, HomeCity = home, GallonsRecieved = gallons });
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
            }

            return null;
        }

        public string NewOdometerReadingConversion(string x)
        {
            int userInputStringtoInt = int.Parse(x);
            int odometerReadingInt = userInputStringtoInt;
            return odometerReadingInt.ToString();
        }

        public int FillUpCountConversion(string firebaseFillupCount)
        {
            int fillUpCountStringToInt = int.Parse(firebaseFillupCount);
            return fillUpCountStringToInt;

        }

        public string GetNewFillUpCount(string firebaseFillUpCount)
        {
            int newFillUpCount = FillUpCountConversion(firebaseFillUpCount);
            newFillUpCount++;
            return newFillUpCount.ToString();
        }

        public async Task<Users> CalculateTotalMoneySpent()
        {
            var person = await firebaseHelper.GetPerson();

            string firebaseMoneySpentValue = person.TotalSpent;
            string moneySpentToBeAdded = AmountSpent.ToString();
            double TotalMoneySpentValue = ((double.Parse(firebaseMoneySpentValue)) + (double.Parse(moneySpentToBeAdded)));
            moneySpentString = TotalMoneySpentValue.ToString();
            return null;
        }

        public async Task<Users> CalculateMPG()
        {
            await GetMileageDifference();
            await GetTotalGallons();

            string mileageDifference = newMileageDifferenceValue;
            string totalGallons = newGallonsValue;
            double totalMPG = ((double.Parse(mileageDifference)) / (double.Parse(totalGallons)));
            int roundedMPG = (int)Math.Round(totalMPG, MidpointRounding.AwayFromZero);
            newMPGValue = roundedMPG.ToString();
            return null;

        }

        public async Task<Users> GetMileageDifference()
        {
            var person = await firebaseHelper.GetPerson();

            string firebaseOdometerValue = person.OdometerReading;
            string newValue = OdometerReading.ToString();
            double newDifference = ((double.Parse(newValue)) - (double.Parse(firebaseOdometerValue)));
            newMileageDifferenceValue = newDifference.ToString();
            return null;
        }

        public async Task<Users> GetTotalGallons()
        {
            var person = await firebaseHelper.GetPerson();

            string firebaseGallonsValue = person.GallonsRecieved;
            string userInputToBeAdded = GallonsReceived.ToString();
            double newTotalGallons = ((double.Parse(userInputToBeAdded)) + (double.Parse(firebaseGallonsValue)));
            newGallonsValue = newTotalGallons.ToString();
            return null;
        }

    }
}