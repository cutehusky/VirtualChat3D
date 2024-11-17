using QFramework;

namespace Core.UserAccountModule.Model
{
    public class FirebaseAuthModel: AbstractModel
    {
        public Firebase.Auth.FirebaseAuth Auth;
        
        public void InitFirebase() {
            Auth =  Firebase.Auth.FirebaseAuth.DefaultInstance;
        }
        
        protected override void OnInit()
        {
            
        }
    }
}