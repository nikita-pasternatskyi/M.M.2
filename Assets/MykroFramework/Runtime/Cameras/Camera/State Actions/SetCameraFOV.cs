using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    [System.Serializable]
    public class SetCameraFOV : CameraAction
    {
        [SerializeField] private AnimatedFloat _fov;

        private float _startFOV;

        public override void Enter()
        {
            _startFOV = Camera.Camera.fieldOfView;
        }

        public override void Act()
        {
            Camera.Camera.fieldOfView = _fov.GetValue(_startFOV, Camera.Camera.fieldOfView);
        }
    }
}
