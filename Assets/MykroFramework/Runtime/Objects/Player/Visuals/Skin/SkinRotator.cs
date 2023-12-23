using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player.Utils;
using MykroFramework.Runtime.Objects.Player.Web;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    public class SkinRotator : MonoBehaviour
    {
        [field: SerializeField] public IMovementInputProvider PlayerInput { get; private set; }
        [field: SerializeField] public WallCheck WallCheck { get; private set; }
        [field: SerializeField] public Transform SwingRotationTransform { get; private set; }
        [field: SerializeField] public Transform Skin { get; private set; }
        [field: SerializeField] public SwingPointFinder WebShooter { get; private set; }
    }
}