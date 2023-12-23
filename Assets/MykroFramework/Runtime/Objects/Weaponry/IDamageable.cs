using UnityEngine;

namespace MykroFramework.Runtime.Objects.Weaponry
{
    public interface IDamageable
    {
        public int CurrentHealth { get; }
        public void TakeDamage(int damage, Vector2 direction);
        public void InstantKill();
    }

    public interface IHealable
    {
        public int CurrentHealth { get; }
        public void Heal(int amount);
    }
}
