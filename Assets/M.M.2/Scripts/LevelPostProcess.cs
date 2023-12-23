using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MM2
{
    public class LevelPostProcess : MonoBehaviour
    {
        [SerializeField] private Vector3 _lightsRotation;
        [SerializeField] private Material _lightsMaterial;
        [SerializeField] private Light[] _lights;
        [SerializeField] private Color _alarmLightColor;
        [SerializeField] private Color _normalLightColor;

        private CoroutineHandle _coroutineHandle;

        [Button]
        private void Process()
        {
            _lights = GetComponentsInChildren<Light>(true);
            foreach (var item in _lights)
            {
                item.transform.rotation = Quaternion.Euler(_lightsRotation);
            }
        }

        public void ChangeLightsColor(Color color)
        {
            if (_coroutineHandle.IsRunning)
                Timing.KillCoroutines(_coroutineHandle);
            _coroutineHandle = Timing.RunCoroutine(ChangeLightsRoutine(color));
        }

        [Button]
        public void ChangeLightsToAlarm()
        {
            ChangeLightsColor(_alarmLightColor);
        }

        [Button]
        public void ChangeLightsToNormal()
        {
            ChangeLightsColor(_normalLightColor);
        }

        private IEnumerator<float> ChangeLightsRoutine(Color color)
        {
            _lightsMaterial.SetColor("_EmissiveColor", color);

            foreach (var item in _lights)
            {
                item.color = color;
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
