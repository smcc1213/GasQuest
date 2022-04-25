using GasQuestApp.Services;
using GasQuestApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;

namespace GasQuestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        IAuth auth;

        string userNameString;
        bool hasProfilePicture;
        bool fillupAt1;
        bool fillupAt0;


        public ProfilePage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            BindingContext = new ProfilePageViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var person = await firebaseHelper.GetPerson();

            userName.Text = person.FirstName + " " + person.LastName;
            userCar.Text = person.Vehicle;
            ChangeVehicleFontSize();
            UserCity.Text = person.HomeCity;
            TotalSpent.Text = "$" + person.TotalSpent;
            OdometerReading.Text = person.OdometerReading;
            userNameString = userName.Text.ToString();
            ChangeUserNameTranslationXandY();

            await CheckforPhoto();
            profilePicture.Source = hasProfilePicture ? person.PathToPhoto : "NewProfilePicPlaceholder.png";
            
            await CheckFillupCount();
            MPG.Text = fillupAt0 ? "MPG will display after two fillups" :
                fillupAt1 ? "MPG will display after next fillup" : person.MPG;
            MPG.FontSize = fillupAt0 || fillupAt1 ? 12 : 16;
        }

        private void ChangeUserNameTranslationXandY()
        {
            int x = 
                userNameString.Length < 8 ? 120 : 
                userNameString.Length < 10 ? 95 : 50;
            userName.TranslateTo(x, 65, 0);
           
        }

        private void ChangeVehicleFontSize()
        {
            int carStringChars = userCar.Text.Length;
            userCar.FontSize = (carStringChars > 17) ? 16 : 19;
        }

        private void MapsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MapsPage());
        }

        private void addVehicle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddVehiclePage());
        }

        private void AddFillup_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddFillupPage());

        }

        public async Task<Users> CheckforPhoto()
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            string isTherePhoto = person.IsPhoto;

            if (user != null)
            {
                hasProfilePicture = isTherePhoto != "false" && (isTherePhoto == "true" ? true : false);
            }
            return null;
        }

        public async Task<Users> CheckFillupCount()
        {
            var user = auth.GetCurrentUser();
            var person = await firebaseHelper.GetPerson();

            string countAmount = person.FillupCount;

            if (user != null)
            {
                fillupAt1 = countAmount == "1" ? true : false;
                fillupAt0 = countAmount == "0" ? true : false;
                
            }

            return null;
        }
        
    }
}