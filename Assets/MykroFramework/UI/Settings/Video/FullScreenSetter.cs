using UnityEngine;

namespace Assets.MykroFramework.UI.Settings.Video
{
    public class FullScreenSetter : MonoBehaviour
    {
        public void SetFullScreen(bool value)
        {
            Screen.fullScreen = value;
        }

        public void SetFullScreen()
        {
            Screen.fullScreen = true;
        }
        public void MinimizeScreen()
        {
            Screen.fullScreen = false;
        }
    }
}
