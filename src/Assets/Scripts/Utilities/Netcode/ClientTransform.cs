using Unity.Netcode.Components;

namespace Utilities.Netcode
{
    public class ClientTransform: NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}