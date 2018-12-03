using UnityEngine;

namespace GvG
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IProjectile
    {
        protected Rigidbody2D rb2d_ = null;
        [SerializeField] protected int damage_ = 1;
        [SerializeField] protected float velocity_ = 10;

        protected void Awake()
        {
            rb2d_ = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 direction)
        {
            rb2d_.velocity = direction * velocity_;
        }

        public void OnImpact(Collision2D collision)
        {
            IProjectile projectile = collision.collider.GetComponent<Bullet>() as IProjectile;
            if (projectile == null)
                Destroy(gameObject);
        }

        public void OnImpact(Collider2D collider)
        {
            IProjectile projectile = collider.GetComponent<Bullet>() as IProjectile;
            if (projectile == null)
                Destroy(gameObject);
        }

        #region Events
        protected void OnDisable()
        {
            rb2d_.velocity = Vector2.zero;
        }
        #endregion
        public int damage { get { return damage_; } }
        public Transform getTransform { get { return transform; } }
    }
}
