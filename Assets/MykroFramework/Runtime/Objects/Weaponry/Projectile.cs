using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Weaponry
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract event Action<Projectile> Destroyed;
    }
}
