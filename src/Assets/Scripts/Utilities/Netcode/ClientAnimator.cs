using Unity.Netcode.Components;

namespace Utilities.Netcode
{
    public class ClientAnimator: NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}