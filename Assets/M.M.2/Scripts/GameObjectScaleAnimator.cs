using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Misc
{

    public class GameObjectScaleAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _time;
        [SerializeField] private bool _useUnscaledTime;
        [SerializeField] private bool _setOriginalScaleOnAwake = true;
        [SerializeField] private bool _resetOriginalScaleOnPlay = false;
        [SerializeField] private Vector3 _originalScale;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _cancelWithGO = true;
        public UnityEvent UnityEvent_Finished;
        public UnityEvent UnityEvent_ReverseFinished;
        private CoroutineHandle _couroutineHandle;

        private void Awake()
        {
            if (_setOriginalScaleOnAwake)
                _originalScale = transform.position;
            if (_playOnAwake)
                Play();
        }

        public void Play()
        {
            if (_couroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(_couroutineHandle);
            }
            if(_cancelWithGO)
                _couroutineHandle = Timing.RunCoroutine(Animate().CancelWith(gameObject));
            else
                _couroutineHandle = Timing.RunCoroutine(Animate());
        }

        public void PlayReverse()
        {
            if (_couroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(_couroutineHandle);
            }
            if (_cancelWithGO)
                _couroutineHandle = Timing.RunCoroutine(Animate(true).CancelWith(gameObject));
            else
                _couroutineHandle = Timing.RunCoroutine(Animate(true));
        }

        private IEnumerator<float> Animate(bool reverse = false)
        {
            float timer = 0;
            float time = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_resetOriginalScaleOnPlay)
                _originalScale = transform.localScale;
            var targetScale = _targetScale;
            while (timer <= _time)
            {
                timer += time;
                float t = timer / _time;
                if (reverse)
                {
                    t = 1 - t;
                }
                float curvedT = _curve.Evaluate(t);
                Vector3 scale = Vector3.Lerp(_originalScale, targetScale, curvedT);
                transform.localScale = scale;
                yield return Timing.WaitForOneFrame;
            }
            UnityEvent_Finished?.Invoke();
            if (reverse)
            {
                UnityEvent_ReverseFinished?.Invoke();
            }
        }
    }
}
