using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    [System.Serializable]
    public class RotateSkinToInput : RotateSkinBase
    {
        [SerializeField] private float _smoothness;

        protected override void OnInit(SOStateMachine.SOStateMachine stateMachine)
        {
            base.OnInit(stateMachine);
        }

        public override void Act()
        {
            if (SkinRotator.PlayerInput.RelativeInput == Vector3.zero)
                return;
            var angle = Vector3.SignedAngle(Vector3.forward, SkinRotator.PlayerInput.RelativeInput, Vector3.up);
            SkinRotator.Skin.localEulerAngles = new Vector3(0, Mathf.LerpAngle(SkinRotator.Skin.localEulerAngles.y, angle, Time.deltaTime * _smoothness), 0);
        }
    }
}