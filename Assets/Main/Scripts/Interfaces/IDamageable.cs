using UnityEngine;

namespace GvG
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
        void Dead();
        int health { get; }
        int maxHealth { get; }
        bool dead { get; }
        Vector2 position { get; }
        Transform damageeTransform { get; }
        string damageeTag { get; }
    }
}