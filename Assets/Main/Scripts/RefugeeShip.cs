using UnityEngine;
using UnityEngine.SceneManagement;
namespace GvG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RefugeeShip : Character
    {
        [SerializeField] int maxCapacity_ = 10;
        [SerializeField] Gombi gombiPrefab_;
        [SerializeField] GombiWorker gombiWorkerPrefab_;
        [SerializeField] GombiShield gombiShieldPrefab_;
        [SerializeField] GombiTractor gombiTractorPrefab_;

        private int currentCapacity_ = 0;
        private int carriedGombis_ = 0;

        override protected void StartUpSequence()
        {
            currentCapacity_ = maxCapacity_;
            if (SceneManager.GetActiveScene().name == "InGame")
                currentTargetPos_ = new Vector2(-30, 4.24f);
        }

        override protected void ResetState()
        {

        }

        override public void TakeDamage(int damage)
        {
            health_ -= damage;
            if (health_ <= 0) Dead();
        }
        override public void Dead()
        {
            rb2d_.gravityScale = .4f;
        }

        public Gombi CheckAddGombi(Collider2D other)
        {
            Gombi gombiToAdd = other.GetComponent<Gombi>();
            if (gombiToAdd != null && roomRemaining > 0 && gombiToAdd.entering)
            {
                return gombiToAdd;
            }
            return null;
        }

        public void AddGombi(Gombi gombiToAdd)
        {
            Destroy(gombiToAdd.gameObject);
            carriedGombis_++;
        }

        public void LetGombiWorkerOut()
        {
            if (carriedGombis_ > 0)
            {
                Instantiate(gombiWorkerPrefab_, transform.position, Quaternion.identity);
                carriedGombis_--;
            }
        }

        public void LetGombiTractorOut()
        {
            if (carriedGombis_ > 0)
            {
                Instantiate(gombiTractorPrefab_, transform.position, Quaternion.identity);
                carriedGombis_--;
            }
        }

        public void LetGombiShieldOut()
        {
            if (carriedGombis_ > 0)
            {
                Instantiate(gombiShieldPrefab_, transform.position, Quaternion.identity);
                carriedGombis_--;
            }
        }

        public bool DamageCheck(Collider2D other)
        {
            bool damageTook = false;
            IProjectile projectile = other.GetComponent<PainBullet>() as IProjectile;
            if (projectile != null)
            {
                damageTook = true;
                TakeDamage(projectile.damage);
            }
            return damageTook;
        }

        public bool HealCheck(Collider2D collider)
        {
            bool healTook = false;
            IProjectile projectile = collider.GetComponent<HealBullet>() as IProjectile;
            if (projectile != null)
            {
                healTook = true;
                Heal(projectile.damage);
            }
            return healTook;
        }
        public void EmitExplosionParticle()
        {

        }

        public bool KillShip(Collider2D collider)
        {
            if (collider.tag == "Ground")
            {
                rb2d_.velocity = Vector2.zero;
                rb2d_.constraints = RigidbodyConstraints2D.FreezeAll;
                EmitExplosionParticle();
                return true;
            }
            return false;
        }

        public bool Escape(Collider2D other)
        {
            if (other.tag == "WinArea")
            {
                return true;
            }
            return false;
        }

        public int maxCapacity { get { return maxCapacity_; } }
        public int currentCapacity { get { return currentCapacity_; } }
        public int roomRemaining { get { return currentCapacity_ - carriedGombis_; } }
        public int carriedGombis { get { return carriedGombis_; } }
    }
}