using System;
using QFramework;

namespace Core.UserAccountModule.Model
{
    public class FirebaseAuthModel: AbstractModel
    {
        public Firebase.Auth.FirebaseAuth Auth;
        
        public void InitFirebase() {
            Auth =  Firebase.Auth.FirebaseAuth.DefaultInstance;
        }
        
        public void GetLoginToken(Action<string> callback)
        {
        }
        
        protected override void OnInit()
        {
            
        }
    }
}