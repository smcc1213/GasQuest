using Firebase.Auth;
using GasQuestApp.iOS;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Foundation;

[assembly: Dependency(typeof(AuthIOS))]
namespace GasQuestApp.iOS
{
    public class AuthIOS : IAuth
    {
        public bool IsSignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                _ = Auth.DefaultInstance.SignOut(out NSError error);
                return error == null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser = await Auth.DefaultInstance.CreateUserAsync(email, password);
                return await newUser.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public string GetCurrentUser()
        {
            var user = Auth.DefaultInstance.CurrentUser;

            string uid = user.Uid;

            return uid;
        }
    }
}
