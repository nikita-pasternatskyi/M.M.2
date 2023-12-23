using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    [System.Serializable]
    public class SetCameraDistance : CameraAction
    {
        [SerializeField] private AnimatedFloat _distance;

        private float _startDistance;

        public override void Enter()
        {
            _startDistance = Camera.SpringArm.targetArmLength;
        }

        public override void Act()
        {
            Camera.SpringArm.targetArmLength = _distance.GetValue(_startDistance, Camera.SpringArm.targetArmLength);
        }
    }
}
