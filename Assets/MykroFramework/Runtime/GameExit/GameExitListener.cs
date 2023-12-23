using UnityEngine;

namespace Assets.MykroFramework.Runtime.GameExit
{
    public class GameExitListener : MonoBehaviour
    {
        public GameExitSO GameExitEventChannel;

        private void OnEnable()
        {
            GameExitEventChannel.GameExited += OnGameExitRequested;
        }

        private void OnDisable()
        {
            GameExitEventChannel.GameExited -= OnGameExitRequested;
        }

        private void OnGameExitRequested()
        {
            Application.Quit();
        }
    }
}
