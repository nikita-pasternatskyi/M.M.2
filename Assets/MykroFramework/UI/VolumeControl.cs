using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MykroFramework.Runtime.UI
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private VolumeType _type;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _defaultValue;

        private const string BGM_KEY = "Ambience";
        private const string SFX_KEY = "SFX";
        private const string MASTER_KEY = "Master";

        private void Start()
        {
            string key = GetCurrentKey();
            var value = PlayerPrefs.GetFloat(key, _defaultValue);
            _slider.value = value;
            ValueChanged(_slider.value);
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(ValueChanged);
        }
        
        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(ValueChanged);
        }

        private string GetCurrentKey()
        {
            switch (_type)
            {
                case VolumeType.Music:
                    return BGM_KEY;
                    break;
                case VolumeType.SFX:
                    return SFX_KEY;
                    break;
                case VolumeType.Master:
                    return MASTER_KEY;
                    break;
            }
            return null;
        }

        private void ValueChanged(float sliderValue)
        {
            float soundValue = Mathf.Log10(sliderValue) * 20;
            var key = GetCurrentKey();
            PlayerPrefs.SetFloat(key, sliderValue);
            _mixer.SetFloat(key, soundValue);
        }

    }
}
