using Core.MVC;
using Firebase.Storage;

namespace Core.FirebaseDatabaseModule.Model
{
    public class FirebaseStorageModel: FirebaseModelBase
    {
        public FirebaseStorage Storage;
        public override void InitFirebase()
        {
            Storage = FirebaseStorage.DefaultInstance;
        }
    }
}