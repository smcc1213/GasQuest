using System.Threading.Tasks;
using GasQuestApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GasQuestApp.ViewModels;
using System;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddVehiclePage : ContentPage
    {
        public AddVehiclePage()
        {
            InitializeComponent();

            BindingContext = new AddVehiclePageViewModel();
        }

        private async void GoBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}