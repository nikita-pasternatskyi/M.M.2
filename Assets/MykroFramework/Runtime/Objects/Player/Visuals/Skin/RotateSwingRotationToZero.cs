using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    [System.Serializable]
    public class RotateSwingRotationToZero : RotateSkinBase
    {
        [SerializeField] private float _smoothness;
        public override void Act()
        {
            SkinRotator.SwingRotationTransform.rotation = Quaternion.Slerp(SkinRotator.SwingRotationTransform.rotation, Quaternion.identity, Time.deltaTime * _smoothness);
        }
    }
}