using Core.AdminModule.Model;
using Core.CharacterCustomizeModule.Model;
using Core.ChatBotModule.Model;
using Core.FirebaseDatabaseModule.Model;
using Core.MessageModule.Model;
using Core.NetworkModule.Model;
using Core.OnlineRuntimeModule.EnvironmentCustomizeModule.Model;
using Core.OnlineRuntimeModule.InputModule.Model;
using Core.OnlineRuntimeModule.RoomManagementModule.Model;
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
            RegisterModel(new RoomDataModel());
            RegisterModel(new RoomEnvironmentDataModel());
            RegisterModel(new MessageDataModel());
            RegisterModel(new FriendDataModel());
            RegisterModel(new GeminiDataModel());
            RegisterModel(new CharacterModelDataModel());
            RegisterModel(new UserAccountDataModel());
            RegisterModel(new SystemInfoDataModel());
            RegisterModel(new FirebaseAuthModel());
            RegisterModel(new FirebaseRealTimeDatabaseModel());
            RegisterModel(new FirebaseStorageModel());
        }
    }
}