using GasQuestApp.Views;
using Xamarin.Forms;

namespace GasQuestApp
{
    public partial class App : Application
    {

        IAuth auth;
        public App()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            
            if (auth.IsSignIn())
            {
                MainPage = new AlreadyLoggedInPage();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
