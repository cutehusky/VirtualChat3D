using Core.UserAccountModule.Model;

namespace Core.FriendModule.Model
{
    public class FriendData : UserAccountData
    {
        public bool IsAccepted;
        public string ChatSessionId;
    }
}