using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Misc
{

    public enum MoveType
    {
        Normal, Rect
    }

    public class GameObjectMoveAnimator : MonoBehaviour
    {
        [SerializeField] private MoveType _moveType;
        [SerializeField] private Vector3 _targetOffset;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private float _time;
        [SerializeField] private bool _useUnscaledTime;
        [SerializeField] private bool _setOriginalPositionOnAwake = true;
        [SerializeField] private bool _resetOriginalPositionOnPlay = false;
        [SerializeField] private Vector3 _originalPosition;
        public UnityEvent UnityEvent_Finished;
        public UnityEvent UnityEvent_ReverseFinished;
        private CoroutineHandle _couroutineHandle;

        private RectTransform _rect;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, transform.position + _targetOffset);
        }

        private void Awake()
        {
            if (_moveType == MoveType.Rect)
                _rect = GetComponent<RectTransform>();
            if (_setOriginalPositionOnAwake)
            {
                switch (_moveType)
                {
                    case MoveType.Normal:
                        _originalPosition = transform.position;
                        break;
                    case MoveType.Rect:
                        _originalPosition = _rect.anchoredPosition;
                        break;
                }
            }
            if (_playOnAwake)
                Play();
        }

        public void Play()
        {
            if (_couroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(_couroutineHandle);
            }
            _couroutineHandle = Timing.RunCoroutine(Animate().CancelWith(gameObject));
        }

        public void PlayReverse()
        {
            if (_couroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(_couroutineHandle);
            }
            _couroutineHandle = Timing.RunCoroutine(Animate(true).CancelWith(gameObject));
        }

        private IEnumerator<float> Animate(bool reverse = false)
        {
            float timer = 0;
            float time = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_resetOriginalPositionOnPlay)
            {
                switch (_moveType)
                {
                    case MoveType.Normal:
                        _originalPosition = transform.position;
                        break;
                    case MoveType.Rect:
                        _originalPosition = _rect.anchoredPosition;
                        break;
                }
            }
            var targetOffset = _targetOffset;
            while (timer <= _time)
            {
                timer += time;
                float t = timer / _time;
                if (reverse)
                {
                    if (!_resetOriginalPositionOnPlay)
                        t = 1 - t;
                    else
                        targetOffset = -_targetOffset;
                }
                float curvedT = _curve.Evaluate(t);
                Vector3 position = Vector3.Lerp(_originalPosition, _originalPosition + targetOffset, curvedT);
                switch (_moveType)
                {
                    case MoveType.Normal:
                        transform.position = position;
                        break;
                    case MoveType.Rect:
                        _rect.anchoredPosition = position;
                        break;
                }
                
                yield return Timing.WaitForOneFrame;
            }
            if (reverse)
            {
                UnityEvent_ReverseFinished?.Invoke();
                yield break;
            }
            UnityEvent_Finished?.Invoke();
        }
    }
}
