using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    class SmoothCharacterControllerFollow : MonoBehaviour
    {
        [SerializeField] private Transform _character;
        [SerializeField] private float _smoothness;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private SpringArm.SpringArm _springArm;

        private Vector3 _latePosition;
        private Vector3 _lerpTarget;

        private void Awake()
        {
            transform.parent = null;
            _offset = _character.transform.position - transform.position;
            _latePosition = _character.transform.position;
        }

        private void Update()
        {
            _lerpTarget = (_character.transform.position + _latePosition) * 0.5f;
            transform.position = Vector3.Lerp(transform.position, _lerpTarget, Time.deltaTime * _smoothness);
            _springArm.Update();
        }

        private void LateUpdate()
        {
            _latePosition = _character.position;
        }
    }
}
