using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlreadyLoggedInPage : ContentPage
    {
        public AlreadyLoggedInPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await loginLabel.ScaleTo(1.5, 1000, Easing.Linear);

            await loginLabel.ScaleTo(1, 1000, Easing.Linear);

            await progressBar.ProgressTo(1, 3000, Easing.Linear);

            Application.Current.MainPage = new AppShell();
        }

    }
}