using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Weaponry
{

    public abstract class Weapon : SerializedMonoBehaviour
    {
        public virtual void EndFire()
        {

        }

        public virtual void HoldingFire()
        {

        }

        public abstract void Fire();
    }
}
