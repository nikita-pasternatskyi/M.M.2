using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.MykroFramework.UI.Settings.Video
{
    public class ResolutionSettingsDropDown : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Dropdown _dropDown;

        private Resolution[] _resolutions;

        private void Awake()
        {
            _dropDown.ClearOptions();

            _resolutions = Screen.resolutions;

            int idx = 0;
            List<string> options = new List<string>(Screen.resolutions.Length);

            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                Resolution res = Screen.resolutions[i];
                options.Add($"{res.width} x {res.height}");
                if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                {
                    idx = i;
                }
            }
            _dropDown.AddOptions(options);
            _dropDown.value = idx;
            _dropDown.RefreshShownValue();
        }
        
        private void OnEnable()
        {
            _dropDown.onValueChanged.AddListener(ChangeResolution);
        }
        
        private void OnDisable()
        {
            _dropDown.onValueChanged.RemoveListener(ChangeResolution);
        }

        private void ChangeResolution(int newIndex)
        {
            Resolution resolution = _resolutions[newIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}
