using Core.MVC;
using Firebase.Database;
using QFramework;

namespace Core.FirebaseDatabaseModule.Model
{
    public class FirebaseRealTimeDatabaseModel: FirebaseModelBase
    {
        public DatabaseReference DatabaseRootReference;
        public override void InitFirebase()
        {
            DatabaseRootReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
    }
}