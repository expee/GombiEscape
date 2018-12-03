using UnityEngine;
using System.Collections;

namespace GvG
{
    public class GombiWorker : Gombi
    {
        public delegate void GombiWorkerDeadEvent();
        public static GombiWorkerDeadEvent gombiWorkerDeadEvent;
        [SerializeField] private float randomRate_ = 1;
        [SerializeField] private float followDistance_ = 1;
        [SerializeField] private float followDistanceRandomness_ = 5;
        [SerializeField] private Muzzle muzzle_ = null;
        private Vector2 randomPosition_ = Vector2.zero;
        private bool working_ = false;
        private new void Start()
        {
            FindRefugeeShip();
            StartWorking();
        }
        private new void Update()
        {
            FindPositionAroundRefugeeShip();
            if (isRefugeeShipDamaged)
                FireWeapon();
            else
                StopFiring();
            transform.rotation = Quaternion.LookRotation(Vector3.forward, refugeeShip_.position - position);
        }

        private new void FixedUpdate()
        {
            MoveTo(currentTargetPos_);
        }
        public void StartWorking()
        {
            working_ = true;
            StartCoroutine(RecalculateRandomPosition());
        }

        public void StopWorking()
        {
            working_ = false;
            StopAllCoroutines();
        }

        public void FindPositionAroundRefugeeShip()
        {
            if (!working_)
                return;
            currentTargetPos_ = (position - refugeeShip_.position).normalized * followDistance_ + refugeeShip_.position;
            currentTargetPos_ += randomPosition_;
        }

        IEnumerator RecalculateRandomPosition()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / randomRate_);
                randomPosition_ = Random.insideUnitCircle.normalized * followDistanceRandomness_;

            }
        }
        private new void OnCollisionEnter2D(Collision2D other)
        {
            bool damaged = DamageCheck(other);
            if (damaged && dead && gombiWorkerDeadEvent != null)
            {
                gombiWorkerDeadEvent.Invoke();
            }
        }
        public void FireWeapon()
        {
            muzzle_.Fire();
        }

        public void StopFiring()
        {
            muzzle_.StopFire();
        }

        private new void OnDisable()
        {
            base.OnDisable();
            StopFiring();
        }
        public bool working { get { return working_; } }
        public bool isRefugeeShipDamaged { get { return refugeeShip_.health < refugeeShip_.maxHealth; } }
    }
}
