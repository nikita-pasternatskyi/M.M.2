using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    [System.Serializable]
    public class SetSocketOffset : CameraAction
    {
        [SerializeField] private AnimatedVector _offset;

        private Vector3 _startOffset;

        public override void Enter()
        {
            _startOffset = Camera.SpringArm.socketOffset;
        }

        public override void Act()
        {
            Camera.SpringArm.socketOffset = _offset.GetValue(_startOffset, Camera.SpringArm.socketOffset);
        }
    }
}
