using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GasQuestApp.ViewModels;
using System;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFillupPage : ContentPage
    {
        public AddFillupPage()
        {
            InitializeComponent();

            BindingContext = new AddFillupPageViewModel();
        }

        private async void GoBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (e.NewTextValue.Contains("."))
            {
                if (e.NewTextValue.Length - 1 - e.NewTextValue.IndexOf(".") > 2)
                {
                    var s = e.NewTextValue.Substring(0, e.NewTextValue.IndexOf(".") + 2 + 1);
                    AmountSpent.Text = s;
                    AmountSpent.SelectionLength = s.Length;
                }
            }

        }
    }
}