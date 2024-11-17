using Unity.Netcode;
using UnityEngine;
using UniVRM10;

namespace Core.OnlineRuntimeModule.CharacterControlModule.Controller
{
    public class PlayerExpressionController: NetworkBehaviour
    {
        public Vrm10Instance vrm10Instance;
        
        public void Relax()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }
        
        public void Angry()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }
        
        public void Surprised()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Sad()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Fun()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }
        
        public void Neutral()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 1);
        }
    }
}