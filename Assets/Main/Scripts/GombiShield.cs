using UnityEngine;

namespace GvG
{
    public class GombiShield : Gombi
    {
        public delegate void GombiShieldDeadEvent();
        public static GombiShieldDeadEvent gombiShieldDeadEvent;
        [SerializeField] float shieldingDistanceMax_ = 5.0f;
        [SerializeField] float shieldingDistanceMin_ = 3.0f;
        private IDamageable gombo_;
        private bool working_ = false;
        public IDamageable FindGombo()
        {
            gombo_ = GameObject.FindGameObjectWithTag("Gombo").GetComponent<Gombo>() as IDamageable;
            return gombo_;
        }
        private new void Start()
        {
            FindRefugeeShip();
            FindGombo();
            StartWorking();
        }

        private new void Update()
        {
            GoToShieldingPosition();
        }

        private new void FixedUpdate()
        {
            MoveTo(currentTargetPos_);
        }
        public void StartWorking()
        {
            working_ = true;
        }

        public void StopWorking()
        {
            working_ = false;
            EnterRefugeeShip();
        }

        private new void OnCollisionEnter2D(Collision2D other)
        {
            bool damaged = DamageCheck(other);
            if (damaged && dead && gombiShieldDeadEvent != null)
            {
                gombiShieldDeadEvent.Invoke();
            }
        }
        public void GoToShieldingPosition()
        {
            if (!working_)
                return;
            Vector2 midPoint = (gombo_.position - refugeeShip_.position) / 2;
            Vector2 fixedPointMax = midPoint.normalized * shieldingDistanceMax_;
            Vector2 fixedPointMin = midPoint.normalized * shieldingDistanceMin_;

            if (midPoint.magnitude > fixedPointMax.magnitude)
                midPoint = fixedPointMax;
            else if (midPoint.magnitude < fixedPointMin.magnitude)
                midPoint = fixedPointMin;
            currentTargetPos_ = midPoint + refugeeShip_.position;
        }

        public bool working { get { return working_; } }
    }
}