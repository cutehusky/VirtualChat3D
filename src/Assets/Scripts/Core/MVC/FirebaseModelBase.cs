using QFramework;

namespace Core.MVC
{
    public abstract class FirebaseModelBase: AbstractModel
    {
        public abstract void InitFirebase();
        protected override void OnInit()
        {
            
        }
    }
}