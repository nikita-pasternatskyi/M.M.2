using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    [CreateAssetMenu(menuName = "Player/Visuals/Skin")]
    public class Skin : ScriptableObject
    {
        public RuntimeSkin SkinPrefab;
    }
}