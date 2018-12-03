using UnityEngine;

namespace GvG
{
    public class GombiTractor : Gombi
    {
        public delegate void GombiTractorDeadEvent();
        public static GombiTractorDeadEvent gombiTractorDeadEvent;
        private ICaptureable gombo_;
        private SpringJoint2D sj2d_;
        [SerializeField] private float keepAwayDistance_ = 12.0f;
        [SerializeField] private float tractorLength_ = 2.0f;
        [SerializeField] private float tractorFrequency_ = 10.0f;
        [SerializeField] private float tractorDamping_ = .4f;
        private bool working_ = false;
        private new void Start()
        {
            FindRefugeeShip();
            FindGombo();
            StartWorking();
        }
        private new void Update()
        {
            if (working)
            {
                if (isTractorAttached)
                {
                    HoldPosition();
                }
                else
                {
                    GrabGombo();
                }
            }
        }

        private new void FixedUpdate()
        {
            MoveTo(currentTargetPos_);
        }
        public ICaptureable FindGombo()
        {
            gombo_ = GameObject.FindGameObjectWithTag("Gombo").GetComponent<Gombo>() as ICaptureable;
            return gombo_;
        }

        public void StartWorking()
        {
            working_ = true;
            GrabGombo();
        }

        public void StopWorking()
        {
            working_ = false;
            Destroy(sj2d_);
            EnterRefugeeShip();
        }

        public void GrabGombo()
        {
            currentTargetPos_ = gombo_.position;
            if (!isTractorAttached && Vector2.Distance(currentTargetPos_, position) < tractorLength_)
            {
                sj2d_ = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
                sj2d_.autoConfigureDistance = false;
                sj2d_.frequency = tractorFrequency_;
                sj2d_.dampingRatio = tractorDamping_;
                sj2d_.distance = tractorLength_;

                sj2d_.connectedBody = gombo_.captureeRigidbody2d;
            }
        }

        private new void OnCollisionEnter2D(Collision2D other)
        {
            bool damaged = DamageCheck(other);
            if (damaged && dead && gombiTractorDeadEvent != null)
            {
                gombiTractorDeadEvent.Invoke();
            }
        }
        private new void OnDisable()
        {
            base.OnDisable();
            Destroy(sj2d_);
        }
        public void HoldPosition()
        {
            Vector2 targetPos = ((gombo_.position - refugeeShip_.position).normalized * keepAwayDistance_) + refugeeShip_.position;
            currentTargetPos_ = targetPos;
        }

        public bool isTractorAttached { get { return sj2d_ != null && sj2d_.connectedBody != null; } }
        public bool working { get { return working_; } }
    }
}
