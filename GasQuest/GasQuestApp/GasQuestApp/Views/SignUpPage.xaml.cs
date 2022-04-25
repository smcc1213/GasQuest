using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GasQuestApp.ViewModels;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        bool eye2 = false;

        public SignUpPage()
        {
            InitializeComponent();

            BindingContext = new SignUpPageViewModel();
        }

        private async void TapGestureRecognizer2_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void showPasswordSignUp_Clicked(object sender, EventArgs e)
        {
            showPasswordButton2.Source = !eye2 ? "eye.png" : "eye_closed.png";
            newPassword.IsPassword = eye2;
            confirmPassword.IsPassword = eye2;
            eye2 = !eye2;
        }
    }
}