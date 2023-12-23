using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    [System.Serializable]
    public class SwingRotate : RotateSkinBase
    {
        [SerializeField] private float _smoothness;
        public override void Act()
        {
            Vector3 direction = (SkinRotator.transform.position - SkinRotator.WebShooter.LastSwingHit).normalized;

            SkinRotator.SwingRotationTransform.rotation = Quaternion.Slerp(SkinRotator.SwingRotationTransform.rotation,
                 Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.forward, -direction), -direction), Time.deltaTime * _smoothness);
        }
    }
}