using System;
using System.Collections;
using UnityEngine;
using UniVRM10;
using Random = UnityEngine.Random;

namespace Core.ChatBotModule.View
{
    public class ExpressionControl : MonoBehaviour
    {
        [SerializeField] private bool isSpeaking = false;
        public Vrm10Instance vrm10Instance;
   
        public void SetSpeak(bool isSpeak)
        {
            isSpeaking = isSpeak;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Aa, 0);
        }

        private float _speakTimeout = 0;
        public void RandomSpeak()
        {
            if (!isSpeaking)
                return;
            if (_speakTimeout <= 0)
            {
                vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Aa, Random.value);
                _speakTimeout = 0.2f;
            }
            _speakTimeout -= Time.deltaTime;
        }
    
        public void Relax()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
            StartCoroutine(Reset());
        }
    
        public void Angry()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
            StartCoroutine(Reset());
        }
    
        public void Surprised()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
            StartCoroutine(Reset());
        }

        public void Sad()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
            StartCoroutine(Reset());
        }

        public void Fun()
        {
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
            StartCoroutine(Reset());
        }

        public IEnumerator Reset()
        {
            yield return new WaitForSeconds(1.5f);
            Neutral();
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
    
        void Update()
        {
            RandomSpeak();
        }
    }
}
