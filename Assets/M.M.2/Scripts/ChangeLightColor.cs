using UnityEngine;

namespace MM2
{
    public class ChangeLightColor : MonoBehaviour
    {
        public Color ColorToChangeTO;
        public Light Target;

        public void Change()
        {
            Target.color = ColorToChangeTO;
        }
    }
}
