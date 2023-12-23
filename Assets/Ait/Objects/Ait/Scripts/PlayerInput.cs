using Assets._Game.Controls;
using MykroFramework.Runtime.Cameras;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Weaponry;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ait
{
    public class PlayerInput : MonoBehaviour, ILookInputProvider
    {
        [SerializeField, Required] private GameMap _gameMap;
        [SerializeField, Required] private Camera _camera;
        [SerializeField, Required] private Transform _facingReference;
        [SerializeField, Required] private AitMainWeapon _mainWeapon;
        [SerializeField, Required] private AitGrappleBeam _grappleBeam;
        [SerializeField] private Vector2 _lookSensitivity;

        public GameMap GameplayMap => _gameMap;

        Vector2 ILookInputProvider.LookInput => LookInput * _lookSensitivity * Time.deltaTime;

        public Vector3 RelativeMovementInput;
        public Vector2 AbsoluteMovementInput;

        public Vector2 LookInput;

        private void Update()
        {
            void TriggerWeapon(ButtonState button, Weapon weapon)
            {
                if (button.WasJustPressed)
                {
                    weapon.Fire();
                }
                else if (button.IsPressed)
                {
                    weapon.HoldingFire();
                }
                else if (button.WasJustReleased)
                {
                    weapon.EndFire();
                }
            }
            
            LookInput = new Vector2
               (_gameMap.CreateAxis(_gameMap.LOOK_RIGHT, _gameMap.LOOK_LEFT),
               _gameMap.CreateAxis(_gameMap.LOOK_UP, _gameMap.LOOK_DOWN));

            var forward = _facingReference.forward.XZ().normalized;
            var right = _facingReference.right.XZ().normalized;

            AbsoluteMovementInput = new Vector2
                (_gameMap.CreateAxis(_gameMap.RIGHT, _gameMap.LEFT), 
                _gameMap.CreateAxis(_gameMap.FORWARD, _gameMap.BACK)).normalized;

            RelativeMovementInput = (forward * AbsoluteMovementInput.y + right * AbsoluteMovementInput.x).normalized;

            TriggerWeapon(_gameMap.GetFireState(), _mainWeapon);
            TriggerWeapon(_gameMap.GetAlt_FireState(), _grappleBeam);

        }
    }
}
