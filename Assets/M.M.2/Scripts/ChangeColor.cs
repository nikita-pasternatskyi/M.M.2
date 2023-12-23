using UnityEngine;
using UnityEngine.UI;

namespace MM2
{
    public class ChangeColor : MonoBehaviour
    {
        public Color ColorToChangeTo;
        public Image Image;

        public void Change()
        {
            Image.color = ColorToChangeTo;
        }
    }
}
