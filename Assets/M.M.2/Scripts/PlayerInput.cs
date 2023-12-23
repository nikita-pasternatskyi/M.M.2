using Assets._Game.Controls;
using MykroFramework.Runtime.Cameras;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MM2
{

    public class PlayerInput : MonoBehaviour, ILookInputProvider, IMovementInputProvider
    {
        [SerializeField, Required] private Gameplay _gameMap;
        [SerializeField, Required] private Camera _camera;
        [SerializeField, Required] private Flashlight _flashLight;
        [SerializeField, Required] private Transform _facingReference;
        [SerializeField] private Vector2 _lookSensitivity;

        public Gameplay GameplayMap => _gameMap;

        Vector2 ILookInputProvider.LookInput => LookInput * _lookSensitivity * Time.deltaTime;

        public Vector3 RelativeInput => RelativeMovementInput;

        public Vector2 AbsoluteInput => AbsoluteMovementInput;

        public Vector3 RelativeMovementInput;
        public Vector2 AbsoluteMovementInput;

        public Vector2 LookInput;

        private void Update()
        {
            LookInput = new Vector2
               (_gameMap.CreateAxis(_gameMap.LOOK_RIGHT, _gameMap.LOOK_LEFT),
               _gameMap.CreateAxis(_gameMap.LOOK_UP, _gameMap.LOOK_DOWN));

            var forward = _facingReference.forward.XZ().normalized;
            var right = _facingReference.right.XZ().normalized;

            AbsoluteMovementInput = new Vector2
                (_gameMap.CreateAxis(_gameMap.RIGHT, _gameMap.LEFT),
                _gameMap.CreateAxis(_gameMap.FORWARD, _gameMap.BACK)).normalized;

            RelativeMovementInput = (forward * AbsoluteMovementInput.y + right * AbsoluteMovementInput.x).normalized;

            if (_flashLight.gameObject.activeSelf == false)
                return;
            if (GameplayMap.GetFlashlightState().WasJustPressed)
            {
                _flashLight.Trigger();
            }
        }
    }
}
