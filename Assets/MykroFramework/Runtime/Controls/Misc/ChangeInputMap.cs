using MykroFramework.Runtime.Controls;
using UnityEngine;

namespace Assets.MykroFramework.Runtime.Controls.Misc
{
    public class ChangeInputMap : MonoBehaviour
    {
        public ButtonMap NewButtonMap;

        private void Start()
        {
            InputRouter.Instance.ChangeButtonMap(NewButtonMap);
        }

        public void Change()
        {
            InputRouter.Instance.ChangeButtonMap(NewButtonMap);
        }

        public void ChangeTo(ButtonMap newMap)
        {
            InputRouter.Instance.ChangeButtonMap(newMap);
        }
    }
}
