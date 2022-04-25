using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasQuestApp.Droid
{
    internal class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {

        private TaskCompletionSource<AuthenticatedUser> _tcs;

        public OnCompleteListener(TaskCompletionSource<AuthenticatedUser> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new AuthenticatedUser();
                    user.Id = doc.Id;
                    user.FirstName = doc.GetString("First Name");
                    user.LastName = doc.GetString("Last Name");
                    _tcs.TrySetResult(user);
                    return;
                }
            }
            _tcs.TrySetResult(default(AuthenticatedUser));
        }
    }
}