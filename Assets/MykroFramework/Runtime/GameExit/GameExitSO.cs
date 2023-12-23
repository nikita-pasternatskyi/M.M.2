using System;
using UnityEngine;

namespace Assets.MykroFramework.Runtime.GameExit
{
    [CreateAssetMenu(menuName = "Game/Game Exit Event Channel")]
    public class GameExitSO : ScriptableObject
    {
        public event Action GameExited;

        public void RequestGameExit()
        {
            GameExited?.Invoke();
        }
    }
}
