using MykroFramework.Runtime.Objects.SOAnimationMachine;
using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    public class PlayerSkin : MonoBehaviour
    {
        [SerializeField] private Skin _startSkin;
        [field: SerializeField] public PlayerCharacter SpiderMan { get; private set; }
        [field: SerializeField] public SOAnimator SOAnimator { get; private set; }
        [SerializeField] private MonoBehaviour[] _componentsToEnable;

        public (Skin skin, RuntimeSkin runtimeSkin) _currentSkin;

        public event Action<Skin, RuntimeSkin> SkinChanged;

        private void Awake()
        {
            foreach (var item in _componentsToEnable)
            {
                item.enabled = false;
            }
            ChangeSkin(_startSkin);
            foreach (var item in _componentsToEnable)
            {
                item.enabled = true;
            }
        }

        public void ChangeSkin(Skin skin)
        {
            if (skin == _currentSkin.skin)
                return;
            if (skin == null)
                return;

            var instance = Instantiate(skin.SkinPrefab, transform);
            Destroy(_currentSkin.runtimeSkin);
            _currentSkin.runtimeSkin = instance;
            _currentSkin.skin = skin;
            SOAnimator.Animancer = _currentSkin.runtimeSkin.AnimancerComponent;
            SkinChanged?.Invoke(_currentSkin.skin, _currentSkin.runtimeSkin);
        }
    }
}