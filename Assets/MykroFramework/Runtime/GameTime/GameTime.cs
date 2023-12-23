using MykroFramework.Runtime.Extensions;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime
{
    [CreateAssetMenu(menuName = "World/Game Time")]
    public class GameTime : ScriptableObject
    {
        [SerializeField] private float _defaultTimeScaleChangeTime;
        private CoroutineHandle _timeScaleChangeCoroutine;

        public void ChangeTimeScale(float newTimeScale)
        {
            ChangeTimeScale(newTimeScale, _defaultTimeScaleChangeTime);
        }

        public void ChangeTimeScale(float newTimeScale, float transitionTime)
        {
            if (_timeScaleChangeCoroutine.IsValid)
                Timing.KillCoroutines(_timeScaleChangeCoroutine);
            _timeScaleChangeCoroutine = Timing.RunCoroutine(ChangeTimeScaleCoroutine(newTimeScale, transitionTime));
        }

        public void RestoreTimeScale()
        {
            RestoreTimeScale(_defaultTimeScaleChangeTime);
        }

        public void RestoreTimeScale(float transitionTime)
        {
            ChangeTimeScale(1, transitionTime);
        }

        private IEnumerator<float> ChangeTimeScaleCoroutine(float targetTimeScale, float transitionTime)
        {
            var startTimeScale = Time.timeScale;
            var timer = 0.0f;

            while (timer <= transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, timer / transitionTime);
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}

