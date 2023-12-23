using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MM2
{
    public class ChangeColorOverTime : MonoBehaviour
    {
        [SerializeField] private Graphic _graphic; 
        [SerializeField] private AnimationCurve _curve;

        public UnityEvent Finished;

        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        [SerializeField] private float _time;

        private CoroutineHandle _coroutine;

        public void Play()
        {
            if (_coroutine.IsRunning)
            {
                Timing.KillCoroutines(_coroutine);
            }
            _coroutine = Timing.RunCoroutine(Change());
        }

        public IEnumerator<float> Change()
        {
            var timer = 0f;

            while (timer < _time)
            {
                _graphic.color = Color.Lerp(_startColor, _endColor, _curve.Evaluate(timer/_time));
                timer += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            Finished?.Invoke();
        }
    }
}
