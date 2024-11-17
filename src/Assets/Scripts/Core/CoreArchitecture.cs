using Core.InputModule.Model;
using Core.NetworkModule.Model;
using Core.UserAccountModule.Model;
using QFramework;

namespace Core
{
    public class CoreArchitecture: Architecture<CoreArchitecture>
    {
        protected override void Init()
        {
            RegisterModel(new PlayerInputAction());
            RegisterModel(new EncryptionProvider());
            RegisterModel(new UserProfileDataModel());
        }
    }
}