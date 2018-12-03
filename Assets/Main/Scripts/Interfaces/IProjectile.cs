using UnityEngine;

namespace GvG
{
    public interface IProjectile
    {
        void Launch(Vector2 direction);
        void OnImpact(Collider2D collider);
        void OnImpact(Collision2D collision);
        int damage { get; }
        Transform getTransform { get; }
    }
}
