using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D
{
    public class ObjectFacing2D : MonoBehaviour
    {
        [SerializeField] private Transform _object;
        [ReadOnly] public float Facing;

        private void Start()
        {
            Facing = _object.right.x;
        }

        public void FaceInput(Vector2 input)
        {
            FaceInput(input.x);
        }

        public void FaceInput(float input)
        {
            if (input * _object.right.x < 0) //if they are not with the same sign
                _object.Rotate(0, 180, 0, Space.Self);
            
            Facing = _object.right.x;
        }
    }
}