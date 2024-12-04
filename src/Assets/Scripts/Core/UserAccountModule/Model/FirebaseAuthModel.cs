using System;
using Core.MVC;
using QFramework;

namespace Core.UserAccountModule.Model
{
    public class FirebaseAuthModel: FirebaseModelBase
    {
        public Firebase.Auth.FirebaseAuth Auth;

        public bool IsSentToken = false;
        
        public override void InitFirebase() {
            Auth =  Firebase.Auth.FirebaseAuth.DefaultInstance;
        }
        
        public void GetLoginToken(Action<string> callback)
        {
        }
    }
}