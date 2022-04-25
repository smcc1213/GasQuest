using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GasQuestApp
{
    public interface IAuth
    {
        Task<string> LoginWithEmailAndPassword(string email, string password);

        Task<string> SignUpWithEmailAndPassword(string email, string password);

        string GetCurrentUser();

        bool SignOut();

        bool IsSignIn();
    }
}
