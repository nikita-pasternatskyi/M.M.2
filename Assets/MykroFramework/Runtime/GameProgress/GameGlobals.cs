using UnityEngine;

namespace MykroFramework.Runtime.GameProgress
{
    [CreateAssetMenu(menuName = "Game/Game Globals")]
    public class GameGlobals : ScriptableObject
    {
        [SerializeField] public GameObject PlayerPrefab;
    }
}