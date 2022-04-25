using GasQuestApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        bool eye = false;

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();
        }

        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }

        private void showPassword_Clicked(object sender, EventArgs e)
        {
            showPasswordButton.Source = !eye ? "eye.png" : "eye_closed.png";
            PasswordInput.IsPassword = eye;
            eye = !eye;
        }
    }
}

