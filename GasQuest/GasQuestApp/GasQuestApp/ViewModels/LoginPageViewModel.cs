using GasQuestApp.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GasQuestApp.ViewModels
{
    class LoginPageViewModel : INotifyPropertyChanged
    {
        IAuth auth;
        private string email;
        private string password;
        public ICommand LoginCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }


        public LoginPageViewModel()
        {
            LoginCommand = new Command(Login);
            auth = DependencyService.Get<IAuth>();
        }

        async void Login(object perameter)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email or Password field can not be empty", "Okay");
            }
            else
            {
                var token = await auth.LoginWithEmailAndPassword(Email, Password);

                if (token != string.Empty)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Authentication Failed", "Email or Password are incorrect", "Ok");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

