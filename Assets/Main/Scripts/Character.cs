using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
namespace GvG
{
    public abstract class Character : MonoBehaviour, IMoveable, IDamageable
    {
        [SerializeField] protected int maxHealth_ = 0;
        [SerializeField] protected float maxSpeed_ = 1;
        [SerializeField] protected float maxForceGiven_ = 1;
        [SerializeField] protected float Kp;
        [SerializeField] protected float Ki;
        [SerializeField] protected float Kd;
        [SerializeField] protected bool isDebuggingPID = false;
        [SerializeField] protected float epsilon_ = 1e-6f;
        protected Rigidbody2D rb2d_ = null;
        protected int health_;
        protected Vector2 currentHeading_;
        protected Vector2 currentTargetPos_;
        protected PID pidControlX_;
        protected PID pidControlY_;
        protected Vector2 forceGiven_;
        protected AudioSource audio_;
        protected void Awake()
        {
            audio_ = GetComponent<AudioSource>();
            pidControlX_ = new PID(Kp, Ki, Kd);
            pidControlY_ = new PID(Kp, Ki, Kd);

            rb2d_ = GetComponent<Rigidbody2D>();
            health_ = maxHealth_;
            currentTargetPos_ = transform.localPosition;
            StartUpSequence();
        }

        abstract protected void StartUpSequence();
        abstract protected void ResetState();

        protected void Update()
        {
            if (isDebuggingPID)
            {
                pidControlX_.Kp = Kp;
                pidControlX_.Ki = Ki;
                pidControlX_.Kd = Kd;
                pidControlY_.Kp = Kp;
                pidControlY_.Ki = Ki;
                pidControlY_.Kd = Kd;
            }
        }

        public void MoveTo(Vector2 target)
        {
            Vector2 currentPos = transform.localPosition;
            Vector2 direction = target - currentPos;
            direction = direction.normalized * Mathf.Min(maxSpeed_, direction.magnitude);
            currentHeading_ = direction;
            SpeedUp(direction);
        }

        public void SpeedUp(Vector2 direction)
        {
            float forceX = pidControlX_.CalculatePID(direction.x, rb2d_.velocity.x, maxForceGiven_);
            float forceY = pidControlY_.CalculatePID(direction.y, rb2d_.velocity.y, maxForceGiven_);

            Vector2 forceGiven = new Vector2(forceX, forceY);
            rb2d_.AddForce(forceGiven);
            forceGiven_ = forceGiven;
        }

        public void ChangeTargetPosition(Vector2 newTarget)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                currentTargetPos_ = newTarget;
        }

        virtual public void Heal(int healthPoint)
        {
            health_ += healthPoint;
            if (health_ > maxHealth_) health_ = maxHealth_;
        }
        virtual public void TakeDamage(int damage)
        {
            health_ -= damage;
            if (health_ == 0) Dead();
        }

        virtual public void Dead()
        {
            if (audio_.clip != null)
                audio_.PlayOneShot(audio_.clip);
            StartCoroutine(DeadDelayImpl());
        }

        protected void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Vector2 currentPos = transform.localPosition;
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(currentPos, currentPos + forceGiven_);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(currentPos, currentPos + currentHeading_);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(currentPos, currentPos + rb2d_.velocity);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(currentTargetPos_, 0.3f);
            }
        }

        IEnumerator DeadDelayImpl()
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }

        public int health { get { return health_; } }
        public int maxHealth { get { return maxHealth_; } }
        public bool dead { get { return health_ == 0; } }
        public Vector2 position { get { return transform.localPosition; } }
        public Transform damageeTransform { get { return transform; } }
        public string damageeTag { get { return tag; } }
        public Vector2 actualMoveDirection { get { return rb2d_.velocity.normalized; } }
        public Vector2 currentTargetPosition { get { return currentTargetPos_; } }
        public bool stopped { get { return rb2d_.velocity.magnitude <= epsilon_; } }
        public Vector2 currentPosition { get { return transform.localPosition; } }
    }
}
