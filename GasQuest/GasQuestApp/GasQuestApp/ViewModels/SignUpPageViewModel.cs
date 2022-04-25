using GasQuestApp.Views;
using Xamarin.Forms;
using System.Windows.Input;
using System.ComponentModel;
using Firebase.Database;
using GasQuestApp.Services;
using Firebase.Database.Query;

namespace GasQuestApp.ViewModels
{

    class SignUpPageViewModel : INotifyPropertyChanged
    {
        IAuth auth;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private string city;
        private string confirmPassword;

        public ICommand SubmitCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        FirebaseClient firebaseClient = new FirebaseClient("https://gasquest-38b4c-default-rtdb.firebaseio.com/");

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged("FirstName"); }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged("LastName"); }
        }
        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged("City"); }
        }
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }

        public SignUpPageViewModel()
        {
            SubmitCommand = new Command(SignUp);
            auth = DependencyService.Get<IAuth>();
        }

        async void SignUp(object perameter)
        {
            var userEmail = Email.ToLower();

            if (!userEmail.EndsWith("gmail.com") && !userEmail.EndsWith("yahoo.com") && !userEmail.EndsWith("outlook.com") && !userEmail.EndsWith("icloud.com"))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid Email", "Okay");
            }
            else
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please have a value for every field", "Okay");
                }
                else if (ConfirmPassword != Password)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Passwords must match", "Ok");
                }

                else
                {
                    var user = auth.SignUpWithEmailAndPassword(Email, Password);


                    if (user != null)
                    {

                        await App.Current.MainPage.DisplayAlert("Success", "New User Created:" + Email, "Ok");


                        await firebaseClient.Child("Users").PostAsync(new Users
                        {
                            FirstName = FirstName.ToString(),
                            LastName = LastName.ToString(),
                            Email = Email.ToString(),
                            Uid = auth.GetCurrentUser(),
                            Vehicle = "Add Vehicle Here",
                            OdometerReading = "0",
                            MPG = "0",
                            TotalSpent = "0",
                            FillupCount = "0",
                            HomeCity = City,
                            GallonsRecieved = "0",
                            PathToPhoto = "null",
                            IsPhoto = "false"
                        });


                        var signOut = auth.SignOut();

                        if (signOut)
                        {
                            Application.Current.MainPage = new LoginPage();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("ERROR", "Something went wrong, please try again", "Ok");
                        }
                    }
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
