using UnityEngine;

namespace MykroFramework.Runtime.Cameras
{
    [System.Serializable]
    public class Vector2Limits : Limits<Vector2>
    {
        public Vector2Limits(Limits<Vector2> limits) : base(limits)
        {
        }

        public Vector2Limits(Vector2 min, Vector2 max) : base(min, max)
        {
        }
    }
}
